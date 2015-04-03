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
                var Output = new ListOfItems(args);

                if (Output.Debug)
                    Console.WriteLine("DEBUG");//Output.Items.ForEach(Console.WriteLine);
                else
                {
                    /*var data = new
                    {
                        twistType = cmds.Next(cmds.FirstOrDefault(item => item.Value == "/t" || item.Value == "/twist").Index),
                        hashType = cmds.FirstOrDefault(item => item.Value == "/h" || item.Value == "/hash"),
                        key = cmds.FirstOrDefault(item => item.Value == "/k" || item.Value == "/key")
                    };

                    var encode = cmds.Action(Encode, "/e", "/encode").FirstOrDefault();
                    var decode = cmds.Action(Decode, "/d", "/decode").FirstOrDefault();
                    var cipher = cmds.Action(item => Cipher(item.Value, data.key != null ? data.key.Value : "Z:W"), "c", "cipher").FirstOrDefault();*/

                    /*Console.WriteLine("{0}", data
                        .Select(o => twistType != null ? Twist(o, twistType.Value) : o)
                        .Select(o => hashType != null ? Hash(o, hashType.Value) : o), verbose);*/
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

        private static string Cipher(string value, string key = null)
        {
            var swap = @"[a-z]:[a-z]".ToRegex().Match(key.ToLower()).Value.Split(':').Select(x => char.Parse(x)).Min();
            var map = string.Join(string.Empty, Const.Alphabet.Split(swap).Select(g => g.Flip())) + swap;
            return string.Join(string.Empty, value.Swap(map));
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