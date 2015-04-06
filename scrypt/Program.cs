using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace scrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(Const.Help);
                return;
            }

            try
            {
                var cmds = args.Select(Const.StringItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
#if DEBUG
                Console.Error.WriteLine(e.StackTrace);
#endif
            }
        }

        private static void Cout(string value, bool verbose = false)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (verbose)
                Console.WriteLine("{0} : {1}", value, value.Length);
            else
                Console.WriteLine(value);
        }

        private static void Cout(IEnumerable<string> values, bool verbose = false)
        {
            values.ToList().ForEach(value => Cout(value, verbose));
        }

        private static string Hash(string value, string type = null)
        {
            using (var algorithm = HashAlgorithm.Create(type ?? string.Empty) ?? new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1));
            }
        }

        private static string Cipher(string value, string key = null)
        {
            var swap = @"[a-z]:[a-z]".ToRegex().Match(key.ToLower()).Value.Split(':').Select(x => char.Parse(x)).Min();
            var map = string.Join(string.Empty, Const.Alphabet.Split(swap).Select(g => g.Reverse())) + swap;
            return string.Join(string.Empty, value.Swap(map));
        }
    }
}