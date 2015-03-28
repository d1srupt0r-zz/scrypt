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
                var cmds = args.ToItems().ToList();

                var debug = cmds.Any(item => item.Command == "debug");
                var verbose = cmds.Any(item => item.Command == "v" || item.Command == "verbose");
                var twist = cmds.FirstOrDefault(item => item.Command == "t" || item.Command == "twist");
                var hash = cmds.FirstOrDefault(item => item.Command == "h" || item.Command == "hash");

                if (debug)
                    output.Append(cmds.Select(item => item.ToString()));
                else
                    output.Append(
                        cmds.Action(Encode, "e", "encode").FirstOrDefault(),
                        cmds.Action(Decode, "d", "decode").FirstOrDefault()
                    );

                if (output.Count > 0)
                {
                    Cout(output
                        .Select(o => twist != null ? Twist(/*o.ToCharArray()*/o, twist.Value) : o)
                        .Select(o => hash != null ? Hash(o, hash.Value) : o), verbose);
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
            using (var algorithm = type != null ? HashAlgorithm.Create(type) : new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1));
            }
        }

        private static string Twist(string value, string type = null)
        {
            switch (Enums.GetEnumValue<Enums.Orientation>(type ?? string.Empty))
            {
                case Enums.Orientation.Flip:
                    return string.Join(string.Empty, value.ToItems().Select(x => value[value.Length - 1 - x.Index]));

                case Enums.Orientation.Scramble:
                    return string.Join(string.Empty, value.ToItems().Select(x => x.Index % 4 == 0
                        ? x.Value.First().SwapCase()
                        : x.Value.First()));

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