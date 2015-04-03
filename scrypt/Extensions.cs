using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace scrypt
{
    public static class Extensions
    {
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

        public static string Flip(this string value)
        {
            return string.Join(string.Empty, value.ToItems().Select(x => value[value.Length - 1 - x.Index]));
        }

        public static string Next<T>(this IList<T> list, int index)
        {
            return list.Count > index + 1 ? list[index + 1].ToString() : string.Empty;
        }

        public static IEnumerable<string> Split(this string value, int size)
        {
            return value.ToItems().Where(item => item.Index % size == 0)
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

        public static IEnumerable<Item> ToItems<T>(this IList<T> list)
        {
            return list.Select((c, i) => new Item { Value = c.ToString(), Index = i });
        }

        public static IEnumerable<Item> ToItems(this string value)
        {
            return value.Select((c, i) => new Item { Value = c.ToString(), Index = i });
        }

        public static Regex ToRegex(this string value)
        {
            return new Regex(@value, RegexOptions.Compiled);
        }

        public static string Twist(this string value)
        {
            return string.Join(string.Empty, value.ToItems().Select(x => x.Index % 4 == 0
                ? x.Value.First().SwapCase()
                : x.Value.First()));
        }
    }
}