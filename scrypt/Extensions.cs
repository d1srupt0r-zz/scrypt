using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace scrypt
{
    public static class Extensions
    {
        public static string Next(this IList<string> list, int index)
        {
            return list.Count > index + 1 ? list[index + 1] : string.Empty;
        }

        public static char SwapCase(this char value)
        {
            return char.IsUpper(value) ? char.ToLower(value) : char.ToUpper(value);
        }

        public static Regex ToRegex(this string value)
        {
            return new Regex(@value, RegexOptions.Compiled);
        }
    }
}