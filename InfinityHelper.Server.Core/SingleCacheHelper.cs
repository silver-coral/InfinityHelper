using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public abstract class SingleCacheHelper<T1, T2>
        where T1 : IComparable
        where T2 : class
    {
        protected static readonly ConcurrentDictionary<T1, T2> _cacheDict = new ConcurrentDictionary<T1, T2>(6, 4096);

        public static void AddOrUpdateCache(T2 d, T1 id)
        {
            _cacheDict.TryAdd(id, d);
        }

        public static bool HasCache(T1 id)
        {
            return _cacheDict.ContainsKey(id);
            //return false;
        }

        public static T2 ClearCache(T1 id)
        {
            T2 value = null;
            _cacheDict.TryRemove(id, out value);
            return value;
        }

        public static T2 LoadCache(T1 id)
        {
            if (!HasCache(id))
            {
                throw new Exception("id");
            }
            T2 value = null;
            _cacheDict.TryGetValue(id, out value);
            return value;
        }

        public static List<T2> GetAll()
        {
            return _cacheDict.Values.ToList();
        }

        public static T2 TryGetValue(T1 id,Func<T1,T2> initFunc)
        {
            if (!HasCache(id))
            {               
                AddOrUpdateCache(initFunc(id), id);
            }
            return LoadCache(id);
        }
    }

    public class CharacterCache : SingleCacheHelper<string, Character>
    {
        public static List<Character> GetAllCharsByLevel()
        {
            return GetAll().OrderByDescending(p => p.Level).ThenByDescending(p => p.Exp).ToList();
        }

        public static List<Character> GetAllCharsBySpeed()
        {
            var cList = GetAll();
            var cdList = CharacterDynamicCache.GetAll();

            foreach(var c in cList)
            {
                var cd = cdList.FirstOrDefault(x => x.CharId == c.Id);
                if(cd != null)
                {
                    c.EPM = cd.EPM;
                }
            }

            return cList.OrderByDescending(p => p.EPM).ToList();
        }

        public static Character GetCharByNo(string no)
        {
            var cList = GetAll();
            return cList.FirstOrDefault(p => p.AccountId == no);
        }
    }
    public class CharacterEquipCache : SingleCacheHelper<string, List<Equipment>> { }
    public class CharacterSkillCache : SingleCacheHelper<string, List<Skill>> { }   
    public class CharacterArmyGroupCache : SingleCacheHelper<string, ArmyGroup> { }
    public class AllMapCache : SingleCacheHelper<string, List<Map>> { }
    public class RealmBonusCache : SingleCacheHelper<string, RealmBonus> { }
    public class AllSyntheticCache: SingleCacheHelper<string, List<Synthetic>> { }
    public class CharacterMarketCache : SingleCacheHelper<string, List<MarketItem>> { }
    public class CharacterConfigCache : SingleCacheHelper<string, CharacterConfig>
    {
        private static string CheckFilePath(string id)
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            directory = Path.Combine(directory, id);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = Path.Combine(directory, "config.json");
            return filePath;
        }

        public static CharacterConfig LoadConfig(string id)
        {
            string filePath = CheckFilePath(id);
            if (!File.Exists(filePath))
            {
                CharacterConfig cfg = new CharacterConfig() { CharId = id };
                File.WriteAllText(filePath, JsonUtil.Serialize(cfg));
                return cfg;
            }
            else
            {
                string json = File.ReadAllText(filePath);
                return JsonUtil.Deserialize<CharacterConfig>(json);
            }
        }

        public static void ClearState(CharacterConfig cfg)
        {
            BattleScheduler.CancelChar(cfg.CharId);
            cfg.IsGuaji = false;
            cfg.CurrentMapId = null;
            SaveConfig(cfg);
        }

        public static void SaveConfig(CharacterConfig cfg)
        {
            string filePath = CheckFilePath(cfg.CharId);
            File.WriteAllText(filePath, JsonUtil.Serialize(cfg));

            AddOrUpdateCache(cfg, cfg.CharId);
        }

        public static List<CharacterConfig> LoadAllConfigs()
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");

            string[] files = Directory.GetFiles(directory, "config.json", SearchOption.AllDirectories);
            foreach(string file in files)
            {
                string json = File.ReadAllText(file);
                CharacterConfig cfg = JsonUtil.Deserialize<CharacterConfig>(json);
                AddOrUpdateCache(cfg, cfg.CharId);
            }

            return GetAll();
        }
    }
    public class CharacterDynamicCache : SingleCacheHelper<string, CharacterDynamic>
    {
        private static string CheckFilePath(string id)
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            directory = Path.Combine(directory, id);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = Path.Combine(directory, "dynamic.json");
            return filePath;
        }

        public static CharacterDynamic LoadDynamic(string id)
        {
            string filePath = CheckFilePath(id);
            if (!File.Exists(filePath))
            {
                CharacterDynamic cfg = new CharacterDynamic() { CharId = id };
                File.WriteAllText(filePath, JsonUtil.Serialize(cfg));
                return cfg;
            }
            else
            {
                string json = File.ReadAllText(filePath);
                return JsonUtil.Deserialize<CharacterDynamic>(json);
            }
        }

        public static void SaveDynamic(CharacterDynamic cfg)
        {
            string filePath = CheckFilePath(cfg.CharId);
            File.WriteAllText(filePath, JsonUtil.Serialize(cfg));

            AddOrUpdateCache(cfg, cfg.CharId);
        }

        public static List<CharacterDynamic> LoadAllDynamics()
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");

            string[] files = Directory.GetFiles(directory, "dynamic.json", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                CharacterDynamic cfg = JsonUtil.Deserialize<CharacterDynamic>(json);
                AddOrUpdateCache(cfg, cfg.CharId);
            }

            return GetAll();
        }
    }
    public class ItemFilterCache : SingleCacheHelper<string, ItemFilterConfig>
    {
        private static string CheckFilePath(string id)
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            directory = Path.Combine(directory, id);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = Path.Combine(directory, "filter.json");
            return filePath;
        }

        public static ItemFilterConfig LoadConfig(string id)
        {
            string filePath = CheckFilePath(id);
            if (!File.Exists(filePath))
            {
                ItemFilterConfig cfg = new ItemFilterConfig() { CharId = id };
                File.WriteAllText(filePath, JsonUtil.Serialize(cfg));
                return cfg;
            }
            else
            {
                string json = File.ReadAllText(filePath);
                return JsonUtil.Deserialize<ItemFilterConfig>(json);
            }
        }

        public static void SaveConfig(ItemFilterConfig cfg)
        {
            string filePath = CheckFilePath(cfg.CharId);
            File.WriteAllText(filePath, JsonUtil.Serialize(cfg));

            AddOrUpdateCache(cfg, cfg.CharId);
        }
    }
}
