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
            bool hash = false, verbose = false;

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
                            break;

                        case "/d":
                        case "/decode":
                            output.Add(Decode(args[cmd.index + 1]));
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
                    output.ForEach(value => Cout(Hash(value), verbose));
                else
                    output.ForEach(value => Cout(value, verbose));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Cout(string value, bool verbose)
        {
            if (verbose)
                Console.WriteLine("{0} : {1}", value, value.Length);
            else
                Console.WriteLine(value);
        }

        private static string Decode(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Encoding.ASCII.GetString(Convert.FromBase64String(value));
        }

        private static string Encode(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
        }

        private static string Hash(string value)
        {
            using (var sha1 = new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(value), 0, value.Length - 1));
            }
        }
    }
}