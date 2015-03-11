using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace scrypt
{
    public class Program
    {
        public static string Help
        {
            get { return @"TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4="; }
        }

        public static void Main(string[] args)
        {
            var output = new List<string>();
            bool hash = false, verbose = false;

            try
            {
                var commands = args.Select((value, index) => new { value = args[index], index });

                // empty commands
                if (!commands.Any())
                    output.Add(Help);

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

        private static string Decode<T>(T value)
        {
            return value == null ? string.Empty : Encoding.ASCII.GetString(Convert.FromBase64String(value.ToString()));
        }

        private static string Encode<T>(T value)
        {
            return value == null ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(value.ToString()));
        }

        private static string Hash(string value)
        {
            using (var algorythem = new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(algorythem.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1));
            }
        }
    }
}