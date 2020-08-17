using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NCB.Core.Extensions
{
    public static class StringExtension
    {
        public static string Join(this IEnumerable<string> arr, string separator)
        {
            return string.Join(separator, arr);
        }

        public static string ConvertToUnSign(this string s)
        {
            if (s != null)
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = s.Normalize(NormalizationForm.FormD).ToLower();
                return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            }
            return null;
        }

        public static string ConvertToSlug(this string str)
        {
            str = str.ConvertToUnSign().Replace(",", string.Empty);
            return str.Split(" ").Join("-");
        }

        public static string TrimAll(this string str)
        {
            return str.Trim().Split(" ").Where(x => x != "").Join(" ");
        }

        public static string ConvertToPrice(this int number)
        {
            if (number != 0)
                return number.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de")) + " đ";

            return string.Empty;
        }
    }
}
