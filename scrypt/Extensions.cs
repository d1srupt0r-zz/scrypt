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
                return list.Where(x => values.Contains(x is Item ? (x as Item).Command : x.ToString())).Select(action);
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

        public static bool IsCommand(this string value)
        {
            return !string.IsNullOrEmpty(value) && @"/[a-z]*".ToRegex().IsMatch(value);
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

        public static char SwapCase(this char value)
        {
            return char.IsUpper(value) ? char.ToLower(value) : char.ToUpper(value);
        }

        public static IEnumerable<Item> ToItems<T>(this IList<T> list)
        {
            return list.Select((c, i) => c.ToString().IsCommand() ? new Item(i, c.ToString(), list.Next(i)) : new Item(i, c.ToString()));
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