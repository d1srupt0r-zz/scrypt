using scrypt.CommandLine;
using scrypt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace scrypt
{
    public class Program
    {
        private static ConsoleColor RandomColor
        {
            get
            {
                var r = new Random(Environment.TickCount).Next(14) + 1;
                return ((ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)))[r];
            }
        }

        public static void Main(string[] args)
        {
            var p = Parse(args);
            var options = Options.GetAll(p);

            if (args.Length == 0)
                Cout(ConsoleColor.Blue, true, Const.Example);
            else if (args.Contains("/help"))
                Options.List.ForEach(o => Cout(ConsoleColor.Yellow, true, o.ToString()));
            else
            {
                var junk = args.String().Split(
                        options.SelectMany(x => x.Cmds).ToArray(),
                        StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .Append(Console.IsInputRedirected ? Console.ReadLine() : null);

                Process(options, junk);

                junk.ToList().ForEach(j => Cout(ConsoleColor.Green, true, "{0} : {1}", j, j.Length));
            }
        }

        private static void Cout(ConsoleColor color, bool writeLine, string format, params object[] values)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (writeLine)
                Console.WriteLine(format, values);
            else
                Console.Write(format, values);
            Console.ForegroundColor = c;
        }

        private static string GetKey(string value, string name)
        {
            if (!Console.IsInputRedirected)
            {
                Cout(ConsoleColor.DarkGray, false, "'{0}' {1} key ", value, name);
                return Console.ReadLine();
            }

            return null;
        }

        private static string[] Parse(params string[] values)
        {
            return values.Select(x => @"(--|-)".ToRegex().Replace(x, "/")).ToArray();
        }

        private static void Process(IEnumerable<Param> options, IList<string> values)
        {
            for (var i = 0; i < values.Count; i++)
            {
                foreach (var option in options)
                {
                    switch (option.Type)
                    {
                        case Enums.ParamType.Command:
                        case Enums.ParamType.Trigger:
                            values[i] = option.Method(values[i], null);
                            break;

                        case Enums.ParamType.Crypto:
                            var key = GetKey(values[i], option.Cmds.Last());
                            values[i] = option.Method(values[i], key);
                            break;
                    }
                }
            }
        }
    }
}