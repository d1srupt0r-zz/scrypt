using scrypt.Utils;
using System;
using System.Linq;

namespace scrypt.CommandLine
{
    public class Terminal
    {
        public static ConsoleColor RandomColor
        {
            get
            {
                var r = new Random(Environment.TickCount).Next(14) + 1;
                return ((ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)))[r];
            }
        }

        public static string In(string format, string value, string name)
        {
            if (!Console.IsInputRedirected)
            {
                Out(ConsoleColor.DarkGray, false, format, value, name);
                return Console.ReadLine();
            }

            return null;
        }

        public static void Out(ConsoleColor color, string format, params object[] values)
        {
            Out(color, true, format, values);
        }

        public static void Out(ConsoleColor color, bool writeLine, string format, params object[] values)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (writeLine)
                Console.WriteLine(format, values);
            else
                Console.Write(format, values);
            Console.ForegroundColor = c;
        }

        public static bool Exists(string[] args, params string[] values)
        {
            return values.Any(value => args.Any(x => (@"[-/]" + value).ToRegex().IsMatch(x)));
        }

        public static string[] Parse(params string[] args)
        {
            return args.Where(x => @"[-/]\S+".ToRegex().IsMatch(x)).ToArray();
        }
    }
}