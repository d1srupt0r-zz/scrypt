using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace scrypt
{
    public class Program
    {
        public static string Alphabet
        {
            get { return @"abcdefghijklmnopqrstuvwxyz"; }
        }

        public static string Help
        {
            get { return @"TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4="; }
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(Help);
                return;
            }

            try
            {
                var output = new List<string>();
                var cmds = args.ToItems();

                var debug = cmds.Any(item => item.Command == "debug");
                var verbose = cmds.Any(item => item.Command == "v" || item.Command == "verbose");

                if (debug)
                    output.AddRange(cmds.Where(item => item.Command != "debug").Select(item => item.ToString()));
                else
                {
                    output.AddRange(cmds.Action(Decode, "d", "decode"));
                    output.AddRange(cmds.Action(Encode, "e", "encode"));
                }

                if (output.Count > 0)
                    output.ForEach(o => Cout(o, verbose));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Cout(string value, bool verbose = false)
        {
            if (value.Length == 0)
                return;

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

        private static string Hash(string value, string type)
        {
            using (var algorithm = HashAlgorithm.Create(type) ?? new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1));
            }
        }

        private static string Rot(string value, string type)
        {
            return string.Empty;
        }

        private static IEnumerable<char> Twist(string value, string type = "Scramble")
        {
            switch (Enums.GetEnumValue<Enums.Orientation>(type))
            {
                case Enums.Orientation.Flip:
                    return value.ToItems().Select(item => value[value.Length - 1 - item.Index]);

                case Enums.Orientation.Scramble:
                    return value.ToItems().Select(item => item.Index % 4 == 0 ? item.Character.SwapCase() : item.Character);

                case Enums.Orientation.Rot:
                    return value.ToItems().Select(item => value[item.Index + 13]);
            }

            return value.ToCharArray();
        }
    }
}