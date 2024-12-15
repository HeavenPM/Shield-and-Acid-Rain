using System;
using System.Globalization;

namespace Extensions
{
    public static class NumberExtensions
    {
        public static string FormatWithSpaces<T>(this T value) where T : struct, IFormattable
        {
            var cultureInfo = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            cultureInfo.NumberFormat.NumberGroupSeparator = " ";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
        
            return value.ToString("#,0.##", cultureInfo);
        }
    }
}