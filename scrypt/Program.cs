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
            var output = new List<string>();
            bool encode = false, decode = false, hash = false, verbose = false;

            try
            {
                var commands = args.Select((value, index) => new { value = args[index], index });

                // command parser
                foreach (var cmd in commands)
                {
                    switch (cmd.value.ToLower())
                    {
                        case "/e":
                        case "/encode":
                            output.Add(Encode(args[cmd.index + 1]));
                            encode = true;
                            break;

                        case "/d":
                        case "/decode":
                            output.Add(Decode(args[cmd.index + 1]));
                            decode = true;
                            break;

                        case "/h":
                        case "/hash":
                            hash = true;
                            break;

                        case "/v":
                        case "/verbose":
                            verbose = true;
                            break;
                    }
                }

                if (hash)
                {
                }

                output.ForEach(Console.WriteLine);

                if (verbose)
                    Console.WriteLine("There are {0} results.", output.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string Decode(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Encoding.ASCII.GetString(Convert.FromBase64String(value));
        }

        private static string Encode(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(value), Base64FormattingOptions.InsertLineBreaks);
        }

        private static byte[] Hash(string value)
        {
            using (var sha1 = new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? null : sha1.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1);
            }
        }
    }
}