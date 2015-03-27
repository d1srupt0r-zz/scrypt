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
            return list.Where(x => values.Contains(list.First() is Item ? (x as Item).Command : x.ToString())).Select(action);
        }

        /*public static IEnumerable<TResult> Action<T, TResult>(this IEnumerable<T> list, Func<T, TResult> action, bool condition)
        {
            return condition ? list.Select(action) : list;
        }*/

        public static bool IsCommand(this string value)
        {
            return @"/[a-z]*".ToRegex().IsMatch(value);
        }

        public static string Next(this IList<string> list, int index)
        {
            return list.Count > index + 1 ? list[index + 1] : string.Empty;
        }

        public static IEnumerable<string> Split(this string value, int size)
        {
            return value.ToItems().Where(item => item.Index % size == 0)
                .Select(item => value.Substring(item.Index, item.Index + size > value.Length
                    ? value.Length - item.Index
                    : size));
        }

        public static char SwapCase(this char value)
        {
            return char.IsUpper(value) ? char.ToLower(value) : char.ToUpper(value);
        }

        public static IEnumerable<Item> ToItems(this IList<string> list)
        {
            return list.Select((c, i) => new Item(i, c, list.Next(i)));
        }

        public static IEnumerable<Item> ToItems(this string value)
        {
            return value.Select((c, i) => new Item(i, c));
        }

        public static Regex ToRegex(this string value)
        {
            return new Regex(@value, RegexOptions.Compiled);
        }
    }
}