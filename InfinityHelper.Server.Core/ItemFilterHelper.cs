using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class ItemFilterHelper
    {
        private static readonly Dictionary<string, string> _equipFilterDict = new Dictionary<string, string>();
        private static readonly Dictionary<string, PropertyInfo> _equipFilterDict2 = new Dictionary<string, PropertyInfo>();

        public static Dictionary<string, string> EquipFilterDict { get { return _equipFilterDict; } }

        static ItemFilterHelper()
        {
            var propList = typeof(ItemBase).GetProperties();
            foreach (var p in propList)
            {
                var attrs = p.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (attrs.Length > 0)
                {
                    var attr = attrs[0] as DisplayNameAttribute;
                    _equipFilterDict[p.Name] = attr.DisplayName;
                    _equipFilterDict2[p.Name] = p;
                }
            }
        }

        public static void SetMagicValue(ItemBase em, string name, object value)
        {
            if (!_equipFilterDict2.ContainsKey(name))
            {
                throw new ArgumentException("filter name");
            }
            var property = _equipFilterDict2[name];
            property.SetValue(em, TypeHelper.ConvertType(property.PropertyType, value));
        }

        public static decimal GetMagicValue(ItemBase em, string name)
        {
            if (!_equipFilterDict2.ContainsKey(name))
            {
                throw new ArgumentException("filter name");
            }
            return Convert.ToDecimal(_equipFilterDict2[name].GetValue(em));
        }        

        public static bool CheckEquipmentFilter(ItemBase e, List<ItemFilter> filterList)
        {
            bool result = false;

            foreach (var filter in filterList)
            {
                bool innerResult = true;
                if (filter.Color != null)
                {
                    innerResult &= e.Color == filter.Color;
                }
                if (filter.RealCategory != null)
                {
                    innerResult &= filter.RealCategory == e.RealCategory;
                }
                foreach (var mf in filter.Items)
                {
                    decimal currentValue = GetMagicValue(e, mf.Name);
                    decimal filterValue = mf.Value;

                    innerResult &= currentValue >= filterValue;
                }

                result |= innerResult;
                if (result)
                {
                    break;
                }
            }

            return result;
        }
    }
}
