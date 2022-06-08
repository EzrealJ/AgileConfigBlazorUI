using System;
using System.ComponentModel;
using System.Linq;

namespace AgileConfig.BlazorUI.Extensions
{
    public static class EnumExtension
    {

        public static string GetDescription(this Enum em)
        {
            Type type = em.GetType();
            System.Reflection.FieldInfo fieldInfo = type.GetField(em.ToString());
            object[] vs = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            var descriptionAttribute = vs?.FirstOrDefault() as DescriptionAttribute;
            return descriptionAttribute?.Description ?? string.Empty;
        }


        public static int GetIntValue(this Enum em) => Convert.ToInt32(em);

        public static string GetValue(this Enum em) => Convert.ToInt32(em).ToString();
    }
}
