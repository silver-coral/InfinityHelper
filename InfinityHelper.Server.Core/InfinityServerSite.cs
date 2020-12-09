using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class InfinityServerSite
    {
        public readonly string LoginPath = "/home/login";
        public readonly string DefaultPath = "/character/detail";
        private static readonly Dictionary<string, Func<InfinityServerSite, InfinityApiBase>> _apiDict = new Dictionary<string, Func<InfinityServerSite, InfinityApiBase>>();

        static InfinityServerSite()
        {
            _apiDict.Add("api/login", s => new InfinityLoginApi(s));
            _apiDict.Add("api/swithmap", s => new InfinitySwithMapApi(s));
            _apiDict.Add("api/guaji", s => new InfinityGuajiApi(s));
            _apiDict.Add("api/cancelguaji", s => new InfinityCancelGuajiApi(s));
            _apiDict.Add("api/clearcache", s => new InfinityClearCacheApi(s));
            _apiDict.Add("api/init", s => new InfinityInitApi(s));
            _apiDict.Add("api/equipon", s => new InfinityEquipOnApi(s));
            _apiDict.Add("api/equipoff", s => new InfinityEquipOffApi(s));
            _apiDict.Add("api/equipbind", s => new InfinityEquipBindApi(s));
            _apiDict.Add("api/equipunbind", s => new InfinityEquipUnBindApi(s));
            _apiDict.Add("api/skillon", s => new InfinitySkillOnApi(s));
            _apiDict.Add("api/skilloff", s => new InfinitySkillOffApi(s));
            _apiDict.Add("api/equipsell", s => new InfinityEquipSellApi(s));
            _apiDict.Add("api/marketbuy", s => new InfinityMarketBuyApi(s));
            _apiDict.Add("api/itemuse", s => new InfinityItemUseApi(s));
            _apiDict.Add("api/skillupgrade", s => new InfinitySkillUpgradeApi(s));
            _apiDict.Add("api/equipupgradematerial", s => new InfinityEquipUpgradeMaterialApi(s));
            _apiDict.Add("api/equipupgrade", s => new InfinityEquipUpgradeApi(s));
            _apiDict.Add("api/attrsupdate", s => new InfinityAttrsUpdateApi(s));
            _apiDict.Add("api/reset", s => new InfinityResetDynamicApi(s));
            _apiDict.Add("api/exchangecode", s => new InfinityExchangeCodeApi(s));
            _apiDict.Add("api/groupcreate", s => new InfinityArmyGroupCreateApi(s));
            _apiDict.Add("api/groupjoin", s => new InfinityArmyGroupJoinApi(s));
            _apiDict.Add("api/groupleave", s => new InfinityArmyGroupLeaveApi(s));
            _apiDict.Add("api/groupremove", s => new InfinityArmyGroupRemoveApi(s));
            _apiDict.Add("api/filtercreate", s => new InfinityFilterCreateApi(s));
            _apiDict.Add("api/filterremove", s => new InfinityFilterRemoveApi(s));
            _apiDict.Add("api/filterclear", s => new InfinityFilterClearApi(s));
            _apiDict.Add("api/filtercopy", s => new InfinityFilterCopyApi(s));
            _apiDict.Add("api/realmmaterial", s => new InfinityRealmMaterialApi(s));
            _apiDict.Add("api/realmup", s => new InfinityRealmUpApi(s));
            _apiDict.Add("api/clearmap", s => new InfinityClearMapItemApi(s));
            _apiDict.Add("api/syntheticmake", s => new InfinitySyntheticMakeApi(s));
            _apiDict.Add("api/clearglobalcache", s => new InfinityClearGlobalCacheApi(s));

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                InfinityServerModelAttribute attr = type.GetCustomAttribute(typeof(InfinityServerModelAttribute)) as InfinityServerModelAttribute;
                if (attr != null)
                {
                    _modelTypeDict.Add(attr.TemplateName.ToLower(), type);
                }
            }
        }

        public ProxyUri Uri { get; private set; }
        public IOwinRequest Request { get; private set; }
        public IOwinResponse Response { get; private set; }

        public InfinityServerSite(ProxyUri uri, IOwinRequest request, IOwinResponse response)
        {
            this.Uri = uri;
            this.Request = request;
            this.Response = response;
            this.RequestOption = GetSourceRequestOption();
        }

        public string CurrentCharId { get; set; }

        public Character CurrentChar
        {
            get
            {
                var c = InitChar();
                c.MergeActivity(InitCharActivity());
                return c;
            }
        }

        //public void RefreshCharActivity()
        //{
        //    CharacterActivityCache.ClearCache(this.CurrentCharId);
        //    this.CurrentChar.MergeActivity(InitCharActivity());
        //}

        public CharacterConfig Config { get; set; }

        public CharacterDynamic Dynamic { get; set; }

        public ItemFilterConfig FilterConfig { get; set; }

        public WebRequestOptions GetSourceRequestOption()
        {
            WebRequestOptions option = new WebRequestOptions(this.Uri.AbsolutePath)
            {
                ContentEncoding = this.MainMapping.Encode,
                ContentType = Request.ContentType,
                Referer = this.MainMapping.Referer,
                Timeout = 20000,
                UserAgent = Request.Headers["User-Agent"],
            };
            foreach (var p in Request.Cookies)
            {
                Cookie cookie = new Cookie(p.Key, p.Value, "/", this.MainMapping.Domain);
                option.CurrentCookies.Add(cookie);
            }
            foreach (string headerKey in Request.Headers.Keys)
            {
                option.Headers.Add(headerKey, Request.Headers[headerKey]);
            }
            return option;
        }

        public WebRequestOptions RequestOption { get; set; }

        public void ClearCookie()
        {
            this.RequestOption.CurrentCookies.Clear();
        }

        public void CopyCookiesToResponse()
        {
            //复制cookie
            foreach (System.Net.Cookie ck in this.RequestOption.CurrentCookies)
            {
                this.Response.Cookies.Append(ck.Name, ck.Value);
            }
        }

        public T PostResult<T>(string path, object postData, bool checkState = true)
        {
            this.RequestOption.RequestUrl = string.Format("{0}{1}", this.MainMapping.Authority, path);
            Resp<T> result = WebRequestUtil.PostResult<Resp<T>>(this.RequestOption, postData);

            if (checkState && !result.IsOk)
            {
                throw new Exception(result.Msg);
            }
            return result.Data;
        }

        public T GetResult<T>(string path, bool checkState = true)
        {
            this.RequestOption.RequestUrl = string.Format("{0}{1}", this.MainMapping.Authority, path);
            Resp<T> result = WebRequestUtil.GetResult<Resp<T>>(this.RequestOption);

            if (checkState && !result.IsOk)
            {
                throw new Exception(result.Msg);
            }
            return result.Data;
        }

        #region 外部API

        public void Login(LoginModel data)
        {
            string path = "/foodie-api/gamepassport/login";
            ClearCookie();

            this.PostResult<LoginResultModel>(path, data);

            CopyCookiesToResponse();
        }

        public List<Synthetic> InitAllSynthetics()
        {
            return AllSyntheticCache.TryGetValue("0", id =>
            {
                string path = string.Format("/foodie-api/gameRealm/getSyntheticList");
                return this.PostResult<List<Synthetic>>(path, null);
            });
        }

        public string SyntheticMake(string sid, int count)
        {
            if (count <= 0)
            {
                count = 1;
            }

            string path = string.Format("/foodie-api/gameRealm/breakThroughTheSynthetic?charaId={0}&syntheticId={1}&syntheticNum={2}", this.CurrentCharId, sid, count);
            return this.PostResult<string>(path, null);
        }

        public RealmBonus InitRealmBonus()
        {
            return RealmBonusCache.TryGetValue(this.CurrentCharId, id =>
            {
                string path = string.Format("/foodie-api/gameRealm/getRealmBonus?charaId={0}", this.CurrentCharId);
                return this.PostResult<RealmBonus>(path, null);
            });
        }

        public string ExchangeCode(string code)
        {
            string path = string.Format("/foodie-api/gamepassport/exchangeCode?charaId={0}&exchangeCode={1}", this.CurrentCharId, code);
            return this.PostResult<string>(path, null);
        }

        public List<MarketItem> InitMarket()
        {
            return CharacterMarketCache.TryGetValue(this.CurrentCharId, id =>
            {
                string path = string.Format("/foodie-api/gameMarket/waresList?charaId={0}", this.CurrentCharId);
                return this.PostResult<List<MarketItem>>(path, null);
            });
        }

        public ArmyGroup InitArmyGroup()
        {
            return CharacterArmyGroupCache.TryGetValue(this.CurrentCharId, id =>
            {
                string path = string.Format("/foodie-api/gameAmry/armyBoundary/?charaId={0}", this.CurrentCharId);
                var group = this.PostResult<ArmyGroup>(path, null, false);
                if (group != null)
                {
                    var captionNo = group.CharaInfoVoList.FirstOrDefault(p => p.IsCaption).CharaNo;
                    var caption = CharacterCache.GetCharByNo(captionNo);
                    group.CaptainName = caption.Name;
                    group.CaptainNo = caption.AccountId;
                    group.CaptionId = caption.Id;
                }
                return group;
            });
        }

        public List<ArmyGroup> QueryAllArmyGroups()
        {
            string path = string.Format("/foodie-api/gameAmry/getArmyList?armyName");
            return this.GetResult<List<ArmyGroup>>(path);
        }

        public BattleResult Battle()
        {
            string path = string.Format("/foodie-api/gameChara/checkMapGenMon?charaId={0}&mapId={1}", this.CurrentCharId, this.Config.CurrentMapId);
            return this.PostResult<BattleResult>(path, null);
        }

        public List<BattleResult> BattleArmyRecord(string no)
        {
            string path = string.Format("/foodie-api/gameAmry/getAmryRecord?charaNo={0}&mapId={1}", no, this.Config.CurrentMapId);
            return this.PostResult<List<BattleResult>>(path, null);
        }

        public List<BattleResult> BattleDungeon()
        {
            string path = string.Format("/foodie-api/gameAmry/checkMapGenMon?charaId={0}&mapId={1}", this.CurrentCharId, this.Config.CurrentMapId);
            return this.PostResult<List<BattleResult>>(path, null);
        }

        public string MarketBuy(string eid, int count)
        {
            if (count <= 0)
            {
                count = 1;
            }

            string path = string.Format("/foodie-api/gameMarket/buyWares?charaId={0}&waresId={1}&waresNum={2}", this.CurrentCharId, eid, count);
            return this.PostResult<string>(path, null);
        }

        public string ItemUse(string eid, int count)
        {
            if (count <= 0)
            {
                count = 1;
            }

            string path = string.Format("/foodie-api/gameCharaEquip/usePropypackage?charaId={0}&packItemId={1}&useNum={2}", this.CurrentCharId, eid, count);
            return this.PostResult<string>(path, null);
        }

        public string EquipUpgrade(string eid)
        {
            string path = string.Format("/foodie-api/gameCharaEquip/equitStrengThen?charaId={0}&packItemId={1}", this.CurrentCharId, eid);
            return this.PostResult<string>(path, null);
        }

        public string RealmUp()
        {
            string path = string.Format("/foodie-api/gameRealm/breakThroughTheRealm?charaId={0}", this.CurrentCharId);
            return this.PostResult<string>(path, null);
        }

        public RealmUpMaterial RealmUpMaterial()
        {
            string path = string.Format("/foodie-api/gameRealm/getCaiRealm?charaId={0}", this.CurrentCharId);
            return this.PostResult<RealmUpMaterial>(path, null);
        }

        public EquipUpMaterial EquipUpMaterial(string eid)
        {
            string path = string.Format("/foodie-api/gameCharaEquip/getCaiEquitStrengThen?charaId={0}&packItemId={1}", this.CurrentCharId, eid);
            return this.PostResult<EquipUpMaterial>(path, null);
        }

        public string EquipSell(string eids)
        {
            //string eid = string.Join(",", eids);
            string path = string.Format("/foodie-api/gameCharaEquip/oneClickSale?charaId={0}&packItemIds={1}", this.CurrentCharId, eids);
            return this.PostResult<string>(path, null);
        }

        public string SkillUpgrade(string sid)
        {
            string path = string.Format("/foodie-api/gameCharaSkill/upgradeSkill?charaId={0}&skillId={1}", this.CurrentCharId, sid);
            return this.PostResult<string>(path, null);
        }

        public string SkillOn(string sid, int type)
        {
            string path = string.Format("/foodie-api/gameCharaSkill/makeSkill/?charaId={0}&skillId={1}&skillType={2}", this.CurrentCharId, sid, type);
            return this.PostResult<string>(path, null);
        }

        public string SkillOff(string sid, int type)
        {
            string path = string.Format("/foodie-api/gameCharaSkill/makeDownSkill/?charaId={0}&skillId={1}&skillType={2}", this.CurrentCharId, sid, type);
            return this.PostResult<string>(path, null);
        }

        public string EquipBind(string eid)
        {
            string path = string.Format("/foodie-api/gameMarket/itemBind?charaId={0}&packItemId={1}&bind=1", this.CurrentCharId, eid);
            return this.PostResult<string>(path, null);
        }

        public string EquipUnBind(string eid)
        {
            string path = string.Format("/foodie-api/gameMarket/itemBind?charaId={0}&packItemId={1}&bind=0", this.CurrentCharId, eid);
            return this.PostResult<string>(path, null);
        }

        public string EquipOff(string eid)
        {
            string path = string.Format("/foodie-api/gameCharaEquip/downEquitBypackage?charaId={0}&equitId={1}", this.CurrentCharId, eid);
            return this.PostResult<string>(path, null);
        }

        public string EquipOn(string eid)
        {
            string path = string.Format("/foodie-api/gameCharaEquip/useEquitBypackage?charaId={0}&packItemId={1}", this.CurrentCharId, eid);
            return this.PostResult<string>(path, null);
        }

        public CharacterActivity InitCharActivity()
        {
            return CharacterActivityCache.TryGetValue(this.CurrentCharId, id =>
            {
                string path = string.Format("/foodie-api/gamepassport/getGameCharacterActivity?charaId={0}", this.CurrentCharId);
                var ca = this.GetResult<CharacterActivity>(path);
                return ca;
            });
        }

        public Character InitChar()
        {
            Character result = CharacterCache.TryGetValue(this.CurrentCharId, id =>
            {
                string path = string.Format("/foodie-api/gamepassport/getGameCharacter?charaId={0}", this.CurrentCharId);
                var c = this.GetResult<Character>(path);  
                return c;
            });

            result.IsGuaji = BattleScheduler.CharDict.ContainsKey(result.Id);
            return result;
        }

        public void AutoSell()
        {
            if (this.FilterConfig.Filters.Count > 0)
            {
                var bagItems = this.QueryBagItems();

                List<ItemBase> filteredItems = new List<ItemBase>();
                foreach (var item in bagItems)
                {
                    if (item.Bind == 0 && item.IsEquip() && !item.IsStrenged())
                    {
                        bool isInBag = ItemFilterHelper.CheckEquipmentFilter(item, this.FilterConfig.Filters);
                        if (!isInBag)
                        {
                            filteredItems.Add(item);
                        }
                    }
                }
                if (filteredItems.Count > 0)
                {
                    this.EquipSell(string.Join(",", filteredItems.Select(p => p.RealId)));
                }
            }
        }

        public List<PackageItem> QueryBagItems()
        {
            string path = string.Format("/foodie-api/gameChara/getCharaPackage?charaId={0}", this.CurrentCharId);
            return this.PostResult<List<PackageItem>>(path, null);
        }

        public List<Equipment> InitCharEquips()
        {
            return CharacterEquipCache.TryGetValue(this.CurrentCharId, id =>
            {
                string path = string.Format("/foodie-api/gameCharaEquip/getCharaEquip?charaId={0}", this.CurrentCharId);
                return this.PostResult<List<Equipment>>(path, null);
            });
        }

        public List<PackageItem> QueryMaterials()
        {
            string path = string.Format("/foodie-api/gameChara/getCharaMaterial?charaId={0}", this.CurrentCharId);
            return this.PostResult<List<PackageItem>>(path, null);
        }

        public List<Map> InitStaticAllMaps()
        {
            return AllMapCache.TryGetValue("0", id =>
            {
                string path = "/foodie-api/gameChara/queryMapList";// "/foodie-api/gameChara/mapDropItems";
                List<Map> mapList = this.GetResult<List<Map>>(path); //this.PostResult<List<Map>>(path, null);
                path = "/foodie-api/gameAmry/queryMapList";
                var dMapList = this.GetResult<List<Map>>(path);
                foreach (var m in dMapList)
                {
                    m.IsDungeon = true;
                    mapList.Add(m);
                }
                return mapList;
            });
        }

        public bool CheckIsGroupCaption()
        {
            var group = InitArmyGroup();
            return group != null && group.CaptionId == this.CurrentCharId;
        }

        public List<Map> InitAllMaps()
        {
            return AllMapCache.TryGetValue(this.CurrentCharId, id =>
            {
                List<Map> mapList = new List<Map>();

                var group = InitArmyGroup();
                if (group != null)
                {
                    string path = "/foodie-api/gameAmry/queryMapList";
                    var dMapList = this.GetResult<List<Map>>(path);
                    foreach (var m in dMapList)
                    {
                        m.IsDungeon = true;
                        mapList.Add(m);
                    }
                }
                else
                {
                    string path = "/foodie-api/gameChara/queryMapList";// "/foodie-api/gameChara/mapDropItems";
                    mapList = this.GetResult<List<Map>>(path); //this.PostResult<List<Map>>(path, null);
                }

                return mapList;
            });
        }

        public List<Skill> InitCharSkills()
        {
            return CharacterSkillCache.TryGetValue(this.CurrentCharId, id =>
            {
                string path1 = string.Format("/foodie-api/gameCharaSkill/getCharaUseSkill/?charaId={0}&skillType=1", this.CurrentCharId);
                var skills1 = this.PostResult<List<Skill>>(path1, null);

                string path2 = string.Format("/foodie-api/gameCharaSkill/getCharaUseSkill/?charaId={0}&skillType=2", this.CurrentCharId);
                var skills2 = this.PostResult<List<Skill>>(path2, null);

                return skills1.Union(skills2).ToList();
            });
        }

        public string AttrsUpdate(int csa, int cda, int cea)
        {
            if (csa >= 0 && cda >= 0 && cea >= 0 && csa + cda + cea > 0)
            {
                this.CurrentChar.Physique += csa;
                this.CurrentChar.Dexterous += cda;
                this.CurrentChar.Spirit += cea;
                this.CurrentChar.ReAttrPoint -= (csa + cda + cea);

                string path = "/foodie-api/gameChara/updateAttrPoint";
                return this.PostResult<string>(path, this.CurrentChar);
            }
            return string.Empty;
        }

        public string ArmyGroupLeave()
        {
            string path = string.Format("/foodie-api/gameAmry/exitAmry/?charaId={0}", this.CurrentCharId);
            return this.PostResult<string>(path, null);
        }

        public string ArmyGroupJoin(string aid)
        {
            string path = string.Format("/foodie-api/gameAmry/playerJoinAmry/?charaId={0}&amryId={1}", this.CurrentCharId, aid);
            return this.PostResult<string>(path, null);
        }

        public string ArmyGroupCreate(string name)
        {
            string path = string.Format("/foodie-api/gameAmry/playerCreateAmry/?charaId={0}&armyName={1}", this.CurrentCharId, name);
            return this.PostResult<string>(path, null);
        }

        public string ArmyGroupRemoveChar(string no)
        {
            string path = string.Format("/foodie-api/gameAmry/kickOutAmry/?charaId={0}&charaNo={1}", this.CurrentCharId, no);
            return this.PostResult<string>(path, null);
        }

        #endregion

        public InfinityServerMapping MainMapping { get { return new InfinityServerMapping(); } }

        protected string TryResolveTemplateName(ProxyUri uri)
        {
            return uri.TemplateName;
        }

        public InfinityApiBase TryResolveApi(ProxyUri uri)
        {
            if (_apiDict.ContainsKey(uri.ApiName))
            {
                return _apiDict[uri.ApiName](this);
            }
            return null;
        }

        /// <summary>
        /// _index => {name}/_index.cshtml
        /// path1 => {name}/path1.cshtml
        /// path1/path2 => {name}/path1/path2.cshtml
        /// </summary>
        /// <param name="proxyPath"></param>
        /// <returns></returns>
        protected virtual string TryGetTemplatePath(ProxyUri uri)
        {
            string result = null;

            string templateName = TryResolveTemplateName(uri);
            if (!string.IsNullOrEmpty(templateName))
            {
                string templateFileName = string.Format("{0}.cshtml", templateName);
                result = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template", templateFileName);
            }

            return result;
        }

        /// <summary>
        /// /{name}/1.jpg => {bin}/{name}/1.jpg
        /// /{name}/path1/1.jpg => {bin}/{name}/path1/1.jpg
        /// /{name}/path2/path1/1.jpg => {bin}/{name}/path1/path2/1.jpg
        /// </summary>
        public virtual string GetProxyFilePath(ProxyUri uri)
        {
            string convertedPath = uri.AbsolutePath.Replace("/", "\\").TrimStart('\\');
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, convertedPath);
            return filePath;
        }

        public virtual bool HasTemplate(ProxyUri uri)
        {
            string templatePath = TryGetTemplatePath(uri);
            return !string.IsNullOrEmpty(templatePath) && File.Exists(templatePath);
        }

        #region Razor     

        private static readonly Dictionary<string, Type> _modelTypeDict = new Dictionary<string, Type>();

        protected InfinityServerModel GetProxyModel(ProxyUri uri)
        {
            string templateName = TryResolveTemplateName(uri);
            if (_modelTypeDict.ContainsKey(templateName))
            {
                return Activator.CreateInstance(_modelTypeDict[templateName], this) as InfinityServerModel;
            }
            return new InfinityServerModel(this);
        }

        public string ParseHtml(ProxyUri uri)
        {
            InfinityServerModel model = GetProxyModel(uri);
            string templatePath = TryGetTemplatePath(uri);
            string layoutPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template", "Shared", "_Layout.cshtml");

            var builder = new CodeBuilder<InfinityServerModel>(templatePath, model);
            builder.LoadLayout(layoutPath);
            string result = builder.Generate();
            return result;
        }

        #endregion
    }
}
