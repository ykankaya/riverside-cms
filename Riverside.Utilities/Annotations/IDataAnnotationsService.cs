using System;
using System.Collections.Generic;
using System.Text;

namespace Riverside.Utilities.Annotations
{
    public interface IDataAnnotationsService
    {
        string GetEnumDisplayName(Type enumType, object enumValue);
        string GetEnumDisplayName<TEnum>(TEnum enumValue);
        string GetPropertyDisplayName(Type itemType, string propertyName);
        string GetPropertyDisplayName<TItem>(string propertyName);
    }
}
