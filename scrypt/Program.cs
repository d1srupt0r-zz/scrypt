﻿using System;
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
            string hashType = string.Empty, twistType = string.Empty, cipherType = string.Empty;
            bool cipher = false, hash = false, twist = false, verbose = false;

            try
            {
                var commands = args.Select((c, i) => new { value = c, index = i }).ToList();

                // empty commands
                if (commands.Count == 0)
                    output.Add(Help);

                // command parser
                foreach (var cmd in commands)
                {
                    switch (cmd.value.ToLower())
                    {
                        case "/c":
                        case "/cipher":
                            cipherType = args.Next(cmd.index);
                            cipher = true;
                            break;

                        case "/d":
                        case "/decode":
                            output.Add(Decode(args.Next(cmd.index)));
                            break;

                        case "/dc":
                        case "/decipher":
                            output.Add(args.Next(cmd.index));
                            break;

                        case "/e":
                        case "/encode":
                            output.Add(Encode(args.Next(cmd.index)));
                            break;

                        case "/h":
                        case "/hash":
                            hashType = args.Next(cmd.index);
                            hash = true;
                            break;

                        case "/s":
                        case "/split":
                            output.AddRange(Split(args.Next(cmd.index), 3));
                            break;

                        case "/se":
                            output.AddRange(Split(args.Next(cmd.index), 3).Select(Encode));
                            break;

                        case "/sd":
                            output.Add(string.Join(string.Empty, Split(args.Next(cmd.index), 4).Select(item => Decode(item.PadRight(4, '=')))));
                            break;

                        case "/t":
                        case "/twist":
                            twistType = args.Next(cmd.index);
                            twist = true;
                            break;

                        case "/v":
                        case "/verbose":
                            verbose = true;
                            break;
                    }
                }

                if (cipher)
                    output.ForEach(item => Cout(string.Join(string.Empty, Decipher(item, cipherType)), verbose));
                else if (hash)
                    output.ForEach(item => Cout(Hash(item, hashType), verbose));
                else if (twist)
                    output.ForEach(item => Cout(string.Join(string.Empty, Twist(item, twistType)), verbose));
                else
                    output.ForEach(item => Cout(item, verbose));
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

        private static IEnumerable<char> Decipher(string value, string type)
        {
            var ciphers = new[] { @"^[a-z]:[a-z]$" };

            var key = ciphers.Select(pattern => pattern.ToRegex().Match(type.ToLower()).Value);

            return value.ToCharArray();
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
            using (var algorythem = HashAlgorithm.Create(type) ?? new SHA1Managed())
            {
                return string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(algorythem.ComputeHash(Encoding.UTF8.GetBytes(value), 0, value.Length - 1));
            }
        }

        private static IEnumerable<string> Split(string value, int size)
        {
            return value.Select((c, i) => new { value = c, index = i })
                .Where(item => item.index % size == 0)
                .Select(item => value.Substring(item.index, item.index + size > value.Length
                    ? value.Length - item.index
                    : size));
        }

        private static IEnumerable<char> Twist(string value, string type)
        {
            switch (Enums.GetEnumValue<Enums.Orientation>(type))
            {
                case Enums.Orientation.Flip:
                    return value.Select((c, i) => new { value = c, index = i })
                        .Select(item => value[value.Length - 1 - item.index]);

                case Enums.Orientation.Scramble:
                    return value.Select((c, i) => new { value = c, index = i })
                        .Select(item => item.index % 4 == 0 ? item.value.SwapCase() : item.value);
            }

            return value.ToCharArray();
        }
    }
}