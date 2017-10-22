using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Riverside.Utilities.Annotations
{
    public class DataAnnotationsService : IDataAnnotationsService
    {
        private string GetDisplayName(MemberInfo memberInfo)
        {
            object[] customAttributes = memberInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (customAttributes != null && customAttributes.Length == 1)
                return ((DisplayAttribute)customAttributes[0]).GetName();
            return memberInfo.Name;
        }

        public string GetPropertyDisplayName(Type itemType, string propertyName)
        {
            PropertyInfo propertyInfo = itemType.GetProperty(propertyName);
            return GetDisplayName(propertyInfo);
        }

        public string GetPropertyDisplayName<TItem>(string propertyName)
        {
            return GetPropertyDisplayName(typeof(TItem), propertyName);
        }
        public string GetEnumDisplayName(Type enumType, object enumValue)
        {
            FieldInfo fieldInfo = enumType.GetField(enumValue.ToString());
            return GetDisplayName(fieldInfo);
        }

        public string GetEnumDisplayName<TEnum>(TEnum enumValue)
        {
            return GetEnumDisplayName(typeof(TEnum), enumValue);
        }
    }
}
