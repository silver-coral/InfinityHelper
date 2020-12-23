using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class BattleScheduler
    {
        private static readonly Dictionary<string, List<BattleResult>> _resultDict = new Dictionary<string, List<BattleResult>>();
        private static readonly ConcurrentDictionary<string, BattleForChar> _charDict = new ConcurrentDictionary<string, BattleForChar>();

        public static ConcurrentDictionary<string, BattleForChar> CharDict { get { return _charDict; } }
        public static int TotalCharCount { get { return _charDict.Count; } }
        public static Dictionary<string, List<BattleResult>> ResultDic { get { return _resultDict; } }

        public static Action<string, BattleResult> OnBattleComplete;
        public static Action<string, string> OnError;

        private static void TriggerBattleComplete(string charId, BattleResult bt)
        {
            if (OnBattleComplete != null)
            {
                OnBattleComplete(charId, bt);
            }
        }

        private static void TriggerError(string charId,string error)
        {
            if(OnError != null)
            {
                OnError(charId, error);
            }
        }

        public static bool AddChar(InfinityServerSite r, bool isDungeon = false)
        {
            BattleForChar dc = new BattleForChar(r, isDungeon);
            bool result = _charDict.TryAdd(r.CurrentCharId, dc);
            if (result)
            {
                dc.OnBattleComplete += TriggerBattleComplete;
                dc.OnError += TriggerError;
                dc.Start();
            }
            return result;
        }

        public static bool CancelChar(string charId)
        {
            BattleForChar dc;
            bool result = _charDict.TryRemove(charId, out dc);
            if (result)
            {
                dc.Cancel();
            }
            return result;
        }

        public static bool IsGuaji(string charId)
        {
            bool result = false;
            BattleForChar dc;
            if(_charDict.TryGetValue(charId,out dc))
            {
                result = (dc.IsDungeon == false);
            }
            return result;
        }

        public static bool IsDungeonGuaji(string charId)
        {
            bool result = false;
            BattleForChar dc;
            if (_charDict.TryGetValue(charId, out dc))
            {
                result = (dc.IsDungeon == true);
            }
            return result;
        }
    }

    public class BattleForChar
    {
        private InfinityServerSite _site;
        private string _charId;
        private bool _isDungeon;
        private readonly CancellationTokenSource _ts;
        private readonly CancellationToken _token;
        public event Action<string, BattleResult> OnBattleComplete;
        public event Action<string, string> OnError;
        public string CharId { get { return _charId; } }
        public bool IsDungeon { get { return this._isDungeon; } }
        

        //public List<BattleResult> CurrentBattle { get; private set; }

        public BattleForChar(InfinityServerSite site, bool isDungeon = false)
        {
            this._site = site;
            this._charId = site.CurrentCharId;
            this._isDungeon = isDungeon;
            this._ts = new CancellationTokenSource();
            this._token = _ts.Token;
        }

        public void Cancel()
        {
            _ts.Cancel();            
        }

        private int ConvertDamage(string d)
        {
            if (string.IsNullOrEmpty(d))
            {
                return 0;
            }
            return d.Split(',').Where(p => !string.IsNullOrEmpty(p)).Select(p => int.Parse(p)).Sum();
        }

        private ICharacter FindBattleChar(BattleResult battle, string name)
        {
            ICharacter relChar = battle.LeftList.FirstOrDefault(p => p.Name == name);
            if (relChar == null)
            {
                relChar = battle.RightList.FirstOrDefault(p => p.Name == name);
            }
            return relChar;
        }

        private void GenerateBattleChars(BattleResult battle, bool isGroup = false, Dictionary<string, int> lifeBeforeDict = null)
        {
            for (var j = 0; j < battle.GameMonList.Count; j++)
            {
                battle.GameMonList[j].BattleCharId = j.ToString();
            }

            if (isGroup)
            {
                var group = this._site.InitArmyGroup();
                battle.LeftList = group.CharaInfoVoList.Select(p => CharacterCache.GetCharByNo(p.CharaNo)).Where(p => p != null).ToList();
            }
            else
            {
                battle.LeftList = new List<Character>() { this._site.CurrentChar };
            }

            battle.LifeBefore = new Dictionary<string, int>();
            battle.LifeAfter = new Dictionary<string, int>();
            if (lifeBeforeDict != null)
            {
                foreach (var p in lifeBeforeDict)
                {
                    battle.LifeBefore.Add(p.Key, p.Value);
                    battle.LifeAfter.Add(p.Key, p.Value);
                }
            }
            else
            {
                foreach (var p in battle.LeftList)
                {
                    battle.LifeBefore[p.Id] = p.Life;
                    battle.LifeAfter[p.Id] = p.Life;
                }
            }
        }

        private void GenerateBattleTurn(BattleResult battle, BattleTurn turn, bool isDirect = true)
        {
            ICharacter attacker = FindBattleChar(battle, turn.Attacker);
            ICharacter target = FindBattleChar(battle, turn.Hinjured);

            if (target != null)
            {
                turn.HpList = new List<BattleTurnHp>()
                {
                    new BattleTurnHp()
                    {
                        id = target.Id,
                        hp = target.Life,
                        chp = turn.SurplusHealth,
                    }
                };

                if (battle.LifeAfter.ContainsKey(target.Id))
                {
                    battle.LifeAfter[target.Id] = turn.SurplusHealth;
                }
            }
            else
            {
                turn.HpList = new List<BattleTurnHp>();
            }

            if (attacker != null)
            {
                if (!battle.Statistics.ContainsKey(attacker.Id))
                {
                    battle.Statistics.Add(attacker.Id, new BattleStatisticsData() { Id = attacker.Id, Name = attacker.Name });
                }

                switch (turn.HurtType)
                {
                    case BattleHurtType.PhysicalDamage:
                        battle.Statistics[attacker.Id].PhysicalDamage += ConvertDamage(turn.Hurt);
                        break;
                    case BattleHurtType.MagicalDamage:
                        battle.Statistics[attacker.Id].MagicalDamage += ConvertDamage(turn.Hurt);
                        break;
                    case BattleHurtType.DotDamage:
                        battle.Statistics[attacker.Id].DotDamage += ConvertDamage(turn.Hurt);
                        break;
                    case BattleHurtType.CounterDamage:
                        battle.Statistics[attacker.Id].CounterDamage += ConvertDamage(turn.Hurt);
                        break;
                    case BattleHurtType.Heal:
                        battle.Statistics[attacker.Id].Heal += ConvertDamage(turn.Hurt);
                        break;                    
                    case null:
                        battle.Statistics[attacker.Id].NormalDamage += ConvertDamage(turn.Hurt);
                        break;
                    default: break;
                }
            }
        }

        private void MergeHpList(List<BattleTurnHp> source, List<BattleTurnHp> newList)
        {
            List<BattleTurnHp> addList = new List<BattleTurnHp>();
            foreach (var x in newList)
            {
                var current = source.FirstOrDefault(j => j.id == x.id);
                if (current != null)
                {
                    current.chp = x.chp;
                }
                else
                {
                    addList.Add(x);
                }
            }
            source.AddRange(addList);
        }

        private void GenerateBattleTurns(BattleResult battle, bool isGroup = false)
        {
            battle.Statistics = new Dictionary<string, BattleStatisticsData>();
            for (int i = 0; i < battle.CombatInfo.Count; i++)
            {
                BattleTurn turn = battle.CombatInfo[i];
                turn.CurrentTurn = i + 1;
                GenerateBattleTurn(battle, turn);

                if (turn.HinjuredConmbatList != null)
                {
                    foreach (var p in turn.HinjuredConmbatList)
                    {
                        GenerateBattleTurn(battle, p, false);

                        MergeHpList(turn.HpList, p.HpList);
                    }
                }
            }
        }

        private void UpdateCharDynamic(BattleResult battle, CharacterDynamic cd, string mapId, bool addExp = true)
        {
            cd.BattleTotalWait += battle.Wait;

            if (this._isDungeon)
            {
                cd.BattleDungeonTotalCount++;               
                cd.BattleDungeonLevelTotalCount++;
                cd.BattleDungeonLevelTotalWait += battle.Wait;
            }
            else
            {
                cd.BattleTotalCount++;                
                cd.BattleLevelTotalCount++;
                cd.BattleLevelTotalWait += battle.Wait;
            }

            if (addExp)
            {
                cd.BattleTotalExp += battle.DropExp;

                if (this.IsDungeon)
                {
                    cd.BattleDungeonLevelTotalExp += battle.DropExp;
                }
                else
                {
                    cd.BattleLevelTotalExp += battle.DropExp;
                }

                CharacterActivityCache.ClearCache(cd.CharId);

                if (battle.UpgradeLev == true)
                {
                    CharacterCache.ClearCache(cd.CharId);
                }
            }            

            if (battle.Success == true)
            {
                if (this._isDungeon)
                {
                    cd.BattleDungeonWinCount++;
                    cd.BattleDungeonLevelWinCount++;
                }
                else
                {
                    cd.BattleWinCount++;
                    cd.BattleLevelWinCount++;
                }
            }

            if (battle.GameItemsList != null)
            {
                var allMaps = this._site.InitAllMaps();
                if (!string.IsNullOrEmpty(mapId))
                {
                    Map map = allMaps.FirstOrDefault(p => p.MapId == mapId);

                    foreach (var item in battle.GameItemsList)
                    {
                        MapItem mi = MapItem.FromItem(item);
                        cd.AppendItem(mapId, mi);

                        if (map != null)//&& !map.ItemsVoList.ContainsKey(mi.ItemId))
                        {
                            map.ItemsVoList[mi.ItemId] = mi;
                        }
                    }
                }
            }

            CharacterDynamicCache.SaveDynamic(cd);
        }

        private void InternalPropcessDungeonBattleResult(List<BattleResult> battleList, string mapId)
        {
            for (var i = 0; i < battleList.Count; i++)
            {
                BattleResult battle = battleList[i];
                BattleResult prevBattle = i > 0 ? battleList[i - 1] : null;

                GenerateBattleChars(battle, true, prevBattle != null ? prevBattle.LifeAfter : null);
                GenerateBattleTurns(battle, true);

                bool addExp = i == battleList.Count - 1;
                UpdateCharDynamic(battle, this._site.Dynamic, mapId, addExp);

                if (this.OnBattleComplete != null)
                {
                    battle.TotalCount = this._site.Dynamic.BattleLevelTotalCount;
                    battle.EPM = this._site.Dynamic.EPM;

                    this.OnBattleComplete(this._charId, battle);
                }
            }

            BattleScheduler.ResultDic[this._site.CurrentCharId] = battleList;
        }

        private void InternalProcessAllyRecord(int count, string mapId)
        {
            var group = this._site.InitArmyGroup();
            foreach (var gc in group.CharaInfoVoList)
            {
                var c = CharacterCache.GetCharByNo(gc.CharaNo);
                if (c != null && c.Id != this._site.CurrentCharId)
                {
                    var allyBattleList = this._site.BattleArmyRecord(gc.CharaNo);
                    for (var i = 0; i < count; i++)
                    {
                        BattleResult battle = allyBattleList[i];
                        BattleResult prevBattle = i > 0 ? allyBattleList[i - 1] : null;
                        GenerateBattleChars(battle, true, prevBattle != null ? prevBattle.LifeAfter : null);
                        GenerateBattleTurns(battle, true);

                        var cd = CharacterDynamicCache.TryGetValue(c.Id, CharacterDynamicCache.LoadDynamic);
                        bool addExp = i == count - 1;
                        UpdateCharDynamic(battle, cd, mapId, addExp);

                        if (this.OnBattleComplete != null)
                        {
                            battle.TotalCount = cd.BattleLevelTotalCount;
                            battle.EPM = cd.EPM;

                            this.OnBattleComplete(c.Id, battle);
                        }
                    }
                    BattleScheduler.ResultDic[c.Id] = allyBattleList.Take(count).ToList();
                }
            }
        }

        private void InternalProcessBattleResult(BattleResult battle, string mapId)
        {
            GenerateBattleChars(battle);
            GenerateBattleTurns(battle);

            if (battle.GameItemsList != null)
            {
                this._site.AutoSell();
            }            

            UpdateCharDynamic(battle, this._site.Dynamic, mapId);

            if (this.OnBattleComplete != null)
            {
                battle.TotalCount = this._site.Dynamic.BattleLevelTotalCount;
                battle.EPM = this._site.Dynamic.EPM;

                this.OnBattleComplete(this._charId, battle);
            }

            BattleScheduler.ResultDic[this._site.CurrentCharId] = new List<BattleResult>() { battle };
        }

        private string EnsureMapId()
        {
            string mapId = null;            
            if (this._isDungeon)
            {
                mapId = this._site.Config.CurrentDungeonMapId;
                if (string.IsNullOrEmpty(mapId))
                {
                    mapId = this._site.InitAllDungeonMaps().First().MapId;
                }
            }
            else
            {
                mapId = this._site.Config.CurrentMapId;
                if (string.IsNullOrEmpty(mapId))
                {
                    mapId = this._site.InitAllSingleMaps().First().MapId;
                }
            }
            return mapId;
        }

        private int InternalBattle()
        {
            string mapId = EnsureMapId();

            int wait = 0;            
            if (this._isDungeon)
            {
                List<BattleResult> battleList = this._site.BattleDungeon();
                InternalPropcessDungeonBattleResult(battleList, mapId);
                InternalProcessAllyRecord(battleList.Count, mapId);
                wait = battleList.Sum(p => p.Wait);                
            }
            else
            {
                BattleResult battle = this._site.Battle();
                InternalProcessBattleResult(battle, mapId);                
                wait = battle.Wait;
            }

            return wait;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalStart()
        {
            try
            {
                int tryTimes = 0;

                while (!_token.IsCancellationRequested) //直到人工取消/出错之前一直运行
                {
                    int sleepTime = 0;

                    try
                    {
                        sleepTime = InternalBattle();
                       

                        //判断一下是否人工取消
                        _token.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException) { } //人工取消
                    catch (Exception ex)
                    {
                        Logger.Error(string.Format("CharNo={0},Name={1},MapId={2},Error={3}", this._site.CurrentChar.AccountId, this._site.CurrentChar.Name, this._site.Config.CurrentMapId, ex.Message));
                        this.OnError(this._charId, ex.Message);

                        if (tryTimes < 10)
                        {
                            sleepTime = 10 * (tryTimes + 1) * 1000; //等10秒重试，而不是简单地退出
                            tryTimes++;
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (sleepTime > 0) //如果是手动取消，这里是0
                    {
                        //SpinWait.SpinUntil(() => _token.IsCancellationRequested, sleepTime);
                        Thread.Sleep(sleepTime);
                    }
                }
            }
            catch (ThreadAbortException) { } //IIS关闭，强行中止线程
            catch (Exception ex)
            {
                Logger.Error(string.Format("CharNo={0},Name={1},MapId={2},Error={3}", this._site.CurrentChar.AccountId, this._site.CurrentChar.Name, this._site.Config.CurrentMapId, ex.ToString()));
                this.OnError(this._charId, ex.Message);

                BattleScheduler.CancelChar(this._charId);
                this._site.Config.IsGuaji = false;
                CharacterConfigCache.SaveConfig(this._site.Config);
            }
        }

        public void Start()
        {
            Logger.Info(string.Format("CharId = {0}，start guaji....", this._charId));

            Task.Factory.StartNew(InternalStart, this._token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
