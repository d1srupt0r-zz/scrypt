using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace scrypt
{
    public static class Extensions
    {
        private static Func<char, int, Item> CharItems = (value, index) => new Item { Value = value.ToString(), Index = index };

        public static string Cipher(this string value, string key = "Z:W")
        {
            var swap = @"[a-z]:[a-z]".ToRegex().Match(key.ToLower()).Value.Split(':').Select(x => char.Parse(x)).Min();
            var map = Const.Alphabet.Split(swap).SelectMany(g => g.Reverse()).String() + swap;
            return value.Swap(map).String();
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

        public static string Flip(this string value)
        {
            return value.Reverse().String();
        }

        public static List<string> Format(this IEnumerable<Item> list)
        {
            if (list.Any(item => item.Value == "/t" || item.Value == "/twist"))
                return list.Select(item => item.Value.Twist()).ToList();

            return list.Select(item => item.Value).ToList();
        }

        public static T Get<T>(this IList<T> list, int index)
        {
            return index > -1 && list.Count > index ? list[index] : default(T);
        }

        public static string Hash(this string value, string type = null)
        {
            using (var algorithm = HashAlgorithm.Create(type ?? string.Empty) ?? new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1));
            }
        }

        public static string Rot(this string value)
        {
            return value.Select(CharItems)
                .Select(x => (int)x.Value.First())
                .Select(x => x + 13)
                .Select(x => (char)x).String();
        }

        public static Func<Item, string> Selector(this string value)
        {
            switch (value)
            {
                case "/e":
                case "/encode":
                    return x => x.Value.Encode();

                case "/d":
                case "/decode":
                    return x => x.Value.Decode();

                case "/c":
                case "/cipher":
                    return x => x.Value.Cipher();

                case "/h":
                case "/hash":
                    return x => x.Value.Hash("md5");

                default:
                    return new Func<Item, string>(x => x.Value);
            }
        }

        public static IEnumerable<string> Split(this string value, int size)
        {
            return value.Select(CharItems).Where(item => item.Index % size == 0)
                .Select(item => value.Substring(item.Index, item.Index + size > value.Length
                    ? value.Length - item.Index
                    : size));
        }

        public static string String<T>(this IEnumerable<T> list, string separator = null)
        {
            return string.Join(separator ?? string.Empty, list);
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

        public static IEnumerable<Item> ToItems(this IList<string> list)
        {
            return list.Select((value, index) => new Item
            {
                Value = value,
                Index = index,
                Convert = list.Get(index - 1).Selector()
            });
        }

        public static Regex ToRegex(this string value)
        {
            return new Regex(@value, RegexOptions.Compiled);
        }

        public static string Twist(this string value)
        {
            return value.Select(CharItems).Select(x => x.Index % 4 == 0
                ? x.Value.First().SwapCase()
                : x.Value.First()).String();
        }
    }
}