using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace NCB.Core.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumDescription(this Enum value)
        {
            DescriptionAttribute[] attributes = null;
            if (value.ToString() != null)
            {
                try
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());
                    attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                }
                catch (NullReferenceException e)
                {
                    throw e;
                }
            }
            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
