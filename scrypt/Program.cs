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
                var output = new List<string>();
                var cmds = args.ToItems().ToList();

                var debug = cmds.Any(item => item.Command == "debug");
                var verbose = cmds.Any(item => item.Command == "v" || item.Command == "verbose");

                var twistType = cmds.FirstOrDefault(item => item.Command == "t" || item.Command == "twist");
                var hashType = cmds.FirstOrDefault(item => item.Command == "h" || item.Command == "hash");
                var cipherKey = cmds.FirstOrDefault(item => item.Command == "k" || item.Command == "key");

                if (debug)
                    output.Append(cmds.Select(item => item.ToString()));
                else
                    output.Append(
                        cmds.Action(Encode, "e", "encode").FirstOrDefault(),
                        cmds.Action(Decode, "d", "decode").FirstOrDefault(),
                        cmds.Action(item => Decipher(item.Value, cipherKey != null ? cipherKey.Value : "Z:W"), "c", "cipher").FirstOrDefault()
                    );

                if (output.Count > 0)
                {
                    Cout(output
                        .Select(o => twistType != null ? Twist(o, twistType.Value) : o)
                        .Select(o => hashType != null ? Hash(o, hashType.Value) : o), verbose);
                }
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

        private static string Decode<T>(T item) where T : Item
        {
            return item == null ? string.Empty : Encoding.ASCII.GetString(Convert.FromBase64String(item.Value));
        }

        private static string Encode<T>(T item) where T : Item
        {
            return item == null ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(item.Value));
        }

        private static string Hash(string value, string type = null)
        {
            using (var algorithm = HashAlgorithm.Create(type ?? string.Empty) ?? new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1));
            }
        }

        private static string Decipher(string value, string type = null)
        {
            var swapKey = @"[a-z]:[a-z]".ToRegex().Match(type.ToLower()).Value.Split(':').Select(x => char.Parse(x)).Min();
            var key = string.Join(string.Empty, Const.Alphabet.Split(swapKey).Select(g => g.Flip())) + swapKey;
            return string.Join(string.Empty, value.Swap(key));
        }

        private static string Twist(string value, string type = null)
        {
            switch (Enums.GetEnumValue<Enums.Orientation>(type ?? string.Empty))
            {
                case Enums.Orientation.Flip:
                    return value.Flip();

                case Enums.Orientation.Scramble:
                    return value.Twist();

                case Enums.Orientation.Rot:
                    return string.Join(string.Empty, value.ToItems()
                        .Select(x => (int)x.Value.First())
                        .Select(x => x + 13)
                        .Select(x => (char)x));
            }

            return value.ToString();
        }
    }
}