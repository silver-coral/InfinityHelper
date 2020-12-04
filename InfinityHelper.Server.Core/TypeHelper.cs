using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfinityHelper.Server.Core
{
    public class TypeHelper
    {
        public static object ConvertType(Type targetType, object value)
        {
            var sourceType = value.GetType();
            if (sourceType == targetType)
            {
                return value;
            }
            else
            {
                if (targetType == typeof(int))
                {
                    return Convert.ToInt32(value);
                }
                else if (targetType == typeof(decimal))
                {
                    return Convert.ToDecimal(value);
                }
                else if (targetType == typeof(bool))
                {
                    return Convert.ToBoolean(value);
                }
                else
                {
                    return Convert.ChangeType(value, targetType);
                }
            }
        }

        public static Func<object, object> CreateGetFunction(Type targetType, string propertyName)
        {
            var property = targetType.GetProperty(propertyName);
            var getMethod = property.GetGetMethod();
            var target = Expression.Parameter(typeof(object), "target");
            var castedTarget = getMethod.IsStatic ? null : Expression.Convert(target, targetType);
            var getProperty = Expression.Property(castedTarget, property);
            var castPropertyValue = Expression.Convert(getProperty, typeof(object));
            return Expression.Lambda<Func<object, object>>(castPropertyValue, target).Compile();
        }

        public static Action<object, object> CreateSetAction(Type targetType, string propertyName)
        {
            var property = targetType.GetProperty(propertyName);
            var setMethod = property.GetSetMethod();
            var target = Expression.Parameter(typeof(object), "target");
            var propertyValue = Expression.Parameter(typeof(object), "value");
            var castedTarget = setMethod.IsStatic ? null : Expression.Convert(target, targetType);
            var castedpropertyValue = Expression.Convert(propertyValue, property.PropertyType);
            var propertySet = Expression.Call(castedTarget, setMethod, castedpropertyValue);
            return Expression.Lambda<Action<object, object>>(propertySet, target, propertyValue).Compile();
        }
    }
}
