using System.Collections.Generic;

namespace scrypt
{
    public static class Extensions
    {
        public static string Next(this IList<string> list, int index)
        {
            return list.Count > index + 1 ? list[index + 1] : string.Empty;
        }

        public static char ChangeCase(this char value)
        {
            return char.IsUpper(value) ? char.ToLower(value) : char.ToUpper(value);
        }
    }
}