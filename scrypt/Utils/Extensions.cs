using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using scrypt.CommandLine;

namespace scrypt.Utils
{
    public static class Extensions
    {
        private static Func<char, int, Item> CharItems = (value, index) => new Item { Value = value.ToString(), Index = index };

        public static IList<T> Append<T>(this IList<T> list, T value) where T : class
        {
            if (value != null)
                list.Add(value);

            return list;
        }

        public static string Cipher(this string value, string key = null)
        {
            var k = key == null ? string.Empty : key.ToLower();
            var swap = char.ToLower(@"[a-z]:[a-z]".ToRegex().IsMatch(k)
                .Default(k, "Z:W", "Cipher format 'A:Z', defaulting to Z:W")
                .Split(':').Select(x => char.Parse(x)).Min());
            var map = Const.Alphabet.Split(swap).SelectMany(g => g.Reverse()).String() + swap;
            return value.Swap(map).String();
        }

        public static string Cleanse(this string value)
        {
            return value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).String("\r\n");
        }

        public static string Decode<T>(this T value)
        {
            var bytes = @"[a-zA-Z0-9+/]{4}".ToRegex().IsMatch(value.ToString())
                .Default(value.ToString(), Convert.FromBase64String, "Invalid Base64 string");
            return bytes != null ? Encoding.Default.GetString(bytes) : value.ToString();
        }

        public static TResult Default<TResult>(this bool condition, string value, Func<string, TResult> onTrue, string message)
        {
            if (condition)
                return onTrue(value);
            else
            {
                Terminal.Out(Theme.Warning, message);
                return default(TResult);
            }
        }

        public static T Default<T>(this bool condition, T trueValue, T falseValue, string message)
        {
            if (condition)
                return trueValue;
            else
            {
                Terminal.Out(Theme.Warning, message);
                return falseValue;
            }
        }

        public static string Encode<T>(this T value)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(value is Item
                ? (value as Item).Value
                : value.ToString()));
        }

        public static bool Exists<T>(this T[] array, string pattern, params string[] values)
        {
            var r = string.Format(values.Length > 0 ? "{0}({1})" : "{0}", pattern, string.Join("|", values));
            return r.ToRegex().Matches(array.String()).Count > 0;
        }

        public static string Flip(this string value)
        {
            return value.Reverse().String();
        }

        public static string Hash(this string value, string type = null)
        {
            using (var algorithm = HashAlgorithm.Create(type ?? string.Empty))
            {
                var a = (algorithm != null).Default(algorithm, new SHA1Managed(), "No Hash Algorithm found, defaulting to SHA1");
                return !string.IsNullOrEmpty(value)
                    ? Convert.ToBase64String(a.ComputeHash(Encoding.Default.GetBytes(value), 0, value.Length - 1))
                    : string.Empty;
            }
        }

        public static string Hex<T>(this T value)
        {
            var val = value.ToString();
            return val.All(x => Uri.IsHexDigit(x))
                ? val.HexString().String()
                : BitConverter.ToString(Encoding.Default.GetBytes(val)).Replace("-", string.Empty);
        }

        public static IEnumerable<char> HexString<T>(this T value)
        {
            var val = value.ToString();
            for (int i = 0; i < val.Length; i += 2)
            {
                var hs = val.Substring(i, 2);
                yield return Convert.ToChar(Convert.ToUInt32(hs, 16));
            }
        }

        public static string Ignore<T>(this T value, params char[] ingore)
        {
            return value.ToString().Where(x => !ingore.Contains(x)).String();
        }

        public static string Limit<T>(this T value, int size = 30)
        {
            var x = value is string ? value as string : value.ToString();
            return x.Length > size ? x.Substring(0, size) + "..." : x;
        }

        public static T[] Parse<T>(this T[] array, string pattern)
        {
            return array.Where(x => pattern.ToRegex().IsMatch(x.ToString())).ToArray();
        }

        public static T[] ReplaceAll<T>(this T[] array, string pattern, string replacement = "")
        {
            return array.Select(x => pattern.ToRegex().Replace(x.ToString(), replacement)).Cast<T>().ToArray();
        }

        public static T[] ReplaceAll<T>(this T[] array, string pattern, Func<T, T> action)
        {
            return array.Select(x => pattern.ToRegex().IsMatch(x.ToString()) ? action(x) : x).ToArray();
        }

        public static IEnumerable<string> Slice(this string value, int size)
        {
            return value.Select(CharItems).Where(item => item.Index % size == 0)
                .Select(item => value.Substring(item.Index, item.Index + size > value.Length
                    ? value.Length - item.Index
                    : size));
        }

        public static string String<T>(this IEnumerable<T> values, string separator = null)
        {
            return string.Join(separator ?? string.Empty, values);
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

        public static char ToggleCase(this char value)
        {
            return char.IsUpper(value) ? char.ToLower(value) : char.ToUpper(value);
        }

        public static Regex ToRegex(this string value)
        {
            return new Regex(@value, RegexOptions.Compiled);
        }

        public static string Twist(this string value)
        {
            return value.Select(CharItems).Select(x => x.Index % 4 == 0
                ? x.Value.First().ToggleCase()
                : x.Value.First()).String();
        }
    }
}