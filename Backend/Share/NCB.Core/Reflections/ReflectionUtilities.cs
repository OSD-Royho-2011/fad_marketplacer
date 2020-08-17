using NCB.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NCB.Core.Reflections
{
    public static class ReflectionUtilities
    {
        public static List<PropertyInfo> GetAllPropertiesOfType(Type type)
        {
            return type.GetProperties().ToList();
        }

        public static List<string> GetAllPropertyNamesOfType(Type type)
        {
            var properties = type.GetProperties();
            return properties.Select(p => p.Name).ToList();
        }

        public static IEnumerable<string> GetAllPropertyNames(this Type type)
        {
            return type.GetProperties().Select(p => p.Name).ToList();
        }
    }
}
