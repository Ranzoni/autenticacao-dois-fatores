﻿using System.ComponentModel;

namespace AutenticacaoDoisFatores.Core.Extensoes
{
    public static class ExtensaoEnum
    {
        public static string? Descricao<T>(this T value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = value.ToString();
            var fieldInfo = value.GetType().GetField(value.ToString() ?? "");

            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes != null && attributes.Length > 0)
                    description = ((DescriptionAttribute)attributes[0]).Description;
            }

            return description;
        }
    }
}
