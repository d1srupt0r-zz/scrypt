using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace scrypt
{
    public static class Extensions
    {
        private static Func<char, int, Item> CharItems = (x, i) => new Item { Value = x.ToString(), Index = i };

        public static IEnumerable<TResult> Action<T, TResult>(this IEnumerable<T> list, Func<T, TResult> action, params string[] values)
        {
            if (values.Length > 0)
                return list.Where(x => values.Contains(x is Item ? (x as Item).Value : x.ToString())).Select(action);
            else
                return list.Select(action);
        }

        public static IList<string> Append(this List<string> list, params string[] values)
        {
            list.AddRange(values.Where(s => !string.IsNullOrEmpty(s)));
            return list;
        }

        public static IList<string> Append(this List<string> list, IEnumerable<string> values)
        {
            list.AddRange(values.Where(s => !string.IsNullOrEmpty(s)));
            return list;
        }

        public static string Decode<T>(this T value)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(value is Item
                ? (value as Item).Value
                : value.ToString()));
        }

        public static string Encode<T>(this T value)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(value is Item
                ? (value as Item).Value
                : value.ToString()));
        }

        public static T Next<T>(this IList<T> list, int index)
        {
            return list.Count > index + 1 ? list[index + 1] : default(T);
        }

        public static string Rot(this string value)
        {
            return string.Join(string.Empty, value.Select(CharItems)
                .Select(x => (int)x.Value.First())
                .Select(x => x + 13)
                .Select(x => (char)x));
        }

        public static IEnumerable<string> Split(this string value, int size)
        {
            return value.Select(CharItems).Where(item => item.Index % size == 0)
                .Select(item => value.Substring(item.Index, item.Index + size > value.Length
                    ? value.Length - item.Index
                    : size));
        }

        public static IEnumerable<char> Swap(this string value, string key)
        {
            foreach (var c in value)
            {
                var i = Const.Alphabet.IndexOf(char.ToLower(c));

                if (char.IsLetter(c))
                    yield return char.IsUpper(c) ? char.ToUpper(key[i]) : key[i];
                else
                    yield return c;
            }
        }

        public static char SwapCase(this char value)
        {
            return char.IsUpper(value) ? char.ToLower(value) : char.ToUpper(value);
        }

        public static Regex ToRegex(this string value)
        {
            return new Regex(@value, RegexOptions.Compiled);
        }

        public static string Twist(this string value)
        {
            return string.Join(string.Empty, value.Select(CharItems).Select(x => x.Index % 4 == 0
                ? x.Value.First().SwapCase()
                : x.Value.First()));
        }
    }
}