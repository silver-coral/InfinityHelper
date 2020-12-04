using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public static class EnumUtil
    {
        /// <summary>
        /// 用于枚举的包含关系
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static bool Contains(this int i, int j)
        {
            return (j & i) == j;
        }

        /// <summary>
        /// 用于枚举的包含关系
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static bool Contains(this long i, long j)
        {
            return (j & i) == j;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static bool Contains<T>(this T i, T j)
            where T : Enum
        {
            return Convert.ToInt64(i).Contains(Convert.ToInt64(j));
        }

        private static Dictionary<string, List<EnumItem>> _enumDict = new Dictionary<string, List<EnumItem>>();

        public static Dictionary<string, List<EnumItem>> EnumDict { get { return _enumDict; } }

        private static string GetFieldDesc(System.Reflection.FieldInfo field_info)
        {
            object[] attrs = field_info.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if ((attrs != null) && (attrs.Length > 0))
            {
                return (attrs[0] as System.ComponentModel.DescriptionAttribute).Description;
            }
            return field_info.Name;
        }

        //public static void BindEnumToDropdown(System.Type type, ComboControl cc)
        //{
        //    List<EnumItem> source = GetEnumItems(type);

        //    foreach (var p in source)
        //    {
        //        cc.Items.Add(new ComboItem()
        //        {
        //            Text = p.Description,
        //            Value = p.Value.ToString(),
        //        });
        //    }
        //}


        /// <summary>
        /// 获取一个枚举类型的列表，用于在dropdownList中显示
        /// </summary>
        /// <returns></returns>
        public static List<EnumItem> GetEnumItems(System.Type type)
        {
            string nameKey = type.FullName.ToUpper();
            if (!_enumDict.ContainsKey(nameKey))
            {
                //System.Type type = typeof(T);
                System.Reflection.FieldInfo[] fields = type.GetFields();

                List<EnumItem> itemList = new List<EnumItem>(fields.Length);
                foreach (System.Reflection.FieldInfo fi in fields)
                {
                    if (fi.FieldType == type)
                    {
                        EnumItem item = new EnumItem();
                        item.Name = fi.Name;
                        item.Value = Convert.ToInt64(fi.GetRawConstantValue());
                        item.Description = GetFieldDesc(fi);

                        if (string.IsNullOrEmpty(item.Description))
                        {
                            item.Description = item.Name;
                        }

                        itemList.Add(item);
                    }
                }
                _enumDict[nameKey] = itemList;
            }
            return _enumDict[nameKey];
        }

        public static List<EnumItem> GetEnumItemsByName(string nameKey)
        {
            string key = nameKey.ToUpper();
            if (!_enumDict.ContainsKey(key))
            {
                throw new Exception(string.Format("Enum未缓存：NameKey = {0}", nameKey));
            }
            return _enumDict[key];
        }

        /// <summary>
        /// 获取枚举值的描述
        /// </summary>
        /// <param name="enumType">指定的枚举类型</param>
        /// <param name="enumValue">枚举类型的值</param>
        /// <returns>枚举值的描述,需用DescriptionAttribute进行描述</returns>
        public static string GetDescription(System.Type enumType, long? enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType不是枚举类型");
            }
            List<EnumItem> items = GetEnumItems(enumType);
            EnumItem item = items.Find(p => p.Value == enumValue);
            if (item == null)
            {
                // throw new ArgumentException("enumType不包括此枚举值");
                return "未知";
            }
            return item == null ? enumValue.ToString() : item.Description;
        }

        /// <summary>
        /// 获取枚举值的描述
        /// </summary>
        /// <param name="enumValue">指定的枚举值: 如SystemParameterEnum.ServerLevel</param>
        /// <returns>枚举值的描述,需用DescriptionAttribute进行描述</returns>
        public static string GetDescription(object enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }

            if (enumValue.GetType().IsEnum)
            {
                return GetDescription(enumValue.GetType(), Convert.ToInt64(enumValue));
            }
            else
            {
                throw new ArgumentException("enumValue不是枚举值");
            }
        }
    }

    /// <summary>
    /// 枚举类项目
    /// </summary>
    public class EnumItem
    {
        /// <summary>
        /// 枚举名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 枚举值
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description { get; set; }
    }
}
