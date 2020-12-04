using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class JsonUtil
    {
        /// <summary>
        /// 获得JSON对象中指定属性的值（如果属性值是复杂对象或数组，直接ToString）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetPropertyValue(JObject jObj, string name)
        {
            object result = null;

            JToken p;
            if (jObj.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out p)) //忽略大小写
            {
                if (p is JValue)
                {
                    result = (p as JValue).Value;
                }
                else
                {
                    result = p.ToString().Replace("\r\n", "");//.Replace("\"", "'");
                }
            }

            return result;
        }

        /// <summary>
        /// 获得JSON对象中指定属性的值（复杂对象）
        /// </summary>
        /// <param name="jObj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static JObject GetProperty(JObject jObj, string name)
        {
            JObject result = null;
            JToken p;
            if (jObj.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out p))
            {
                if (p is JObject)
                {
                    result = p as JObject;
                }
            }
            return result;
        }

        /// <summary>
        /// 获得JSON对象中指定属性的值（数组）
        /// </summary>
        /// <param name="jObj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static JArray GetPropertyArray(JObject jObj, string name)
        {
            JArray result = null;
            JToken p;
            if (jObj.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out p))
            {
                if (p is JArray)
                {
                    result = p as JArray;
                }
            }
            return result;
        }

        /// <summary>
        /// json字符串 => javascript对象字符串（生成前端代码时用）
        /// </summary>
        /// <param name="json">json</param>  
        /// <param name="valueSingleQuote">字符串值是否用单引号</param>
        public static string JsonToJsObject(string json, bool valueSingleQuote = true)
        {
            //去掉属性名的双引号，注意使用\s*来兼容空格
            string pattern = "\"(\\w+)\"(\\s*:\\s*)";
            string replacement = "$1$2";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
            json = rgx.Replace(json, replacement);

            //字符串值用单引号
            if (valueSingleQuote)
            {
                pattern = "(\\s*:\\s*)\"(.*?)\"";  //注意匹配值不能用\w+，因为可能有特殊字符，也有可能没值，所以用.*，但不能使用贪婪策略，所以要加?
                replacement = "$1'$2'";
                rgx = new System.Text.RegularExpressions.Regex(pattern);
                json = rgx.Replace(json, replacement);
            }

            return json;
        }

        /// <summary>
        /// JObject =>  javascript对象字符串（生成前端代码时用）
        /// </summary>
        /// <param name="jObj"></param>
        /// <param name="valueSingleQuote"></param>
        /// <returns></returns>
        public static string JObjectToJsObject(JObject jObj, bool valueSingleQuote = true)
        {
            if (jObj == null || !jObj.HasValues)
            {
                return "{}";
            }
            return JsonToJsObject(jObj.ToString().Replace("\r\n", ""), valueSingleQuote);
        }

        /// <summary>
        /// JArray => javascript数组字符串（生成前端代码时用）,要求JArray里是对象JObject
        /// </summary>
        /// <param name="jArray"></param>
        /// <param name="valueSingleQuote"></param>
        /// <returns></returns>
        public static string JArrayToJsArray(JArray jArray, bool valueSingleQuote = true)
        {
            if (jArray == null || !jArray.HasValues)
            {
                return "[]";
            }
            var strList = jArray.Select(p => JObjectToJsObject(p as JObject, valueSingleQuote));
            return string.Format("[{0}]", string.Join(",", strList));
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj, bool camelCase = false)
        {
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();

            settings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; //TODO：如果为null值的属性，是忽略还是保留？
            settings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include; //

            if (camelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //属性名采用小驼峰模式，适用于前端
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 序列化成js对象（生成前端代码时用）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeJsObject(object obj, bool valueSingleQuote = true)
        {
            if (obj == null)
            {
                return "{}";
            }

            string json = Serialize(obj, true);
            return JsonToJsObject(json, valueSingleQuote);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Deserialize(string json, Type type = null)
        {
            if (type != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            }
        }
    }
}
