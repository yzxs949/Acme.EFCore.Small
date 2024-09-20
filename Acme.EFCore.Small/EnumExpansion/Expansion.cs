using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System;
using System.Linq;

namespace EnumExpansion
{
    public static class Expansion
    {
        public static Dictionary<TEnum, string> ToDictionary<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .ToDictionary(e => e, e => e.ToDescription());
        }

        public static List<string> ToDescriptionList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(e => e.ToDescription())
                       .ToList();
        }

        public static List<TEnum> ToEnumList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .ToList();
        }

        public static string ToDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
