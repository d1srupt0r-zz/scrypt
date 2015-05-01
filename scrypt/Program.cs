using scrypt.CommandLine;
using scrypt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace scrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
                Terminal.Out(ConsoleColor.Blue, Const.Example);
            else if (args.Contains("/help"))
                Options.List.ForEach(o => Terminal.Out(ConsoleColor.Yellow, o.ToString()));
            else
            {
                var junk = args.String().Split(Terminal.Parse(args), StringSplitOptions.RemoveEmptyEntries).ToList()
                    .Append(Console.IsInputRedirected ? Console.ReadLine() : null);

                var verbose = Terminal.Exists(args, "v", "verbose");
                Process(Options.GetAll(args), junk, verbose);

                junk.ToList().ForEach(j => Terminal.Out(ConsoleColor.Green, verbose ? "{0} : {1}" : "{0}", j, j.Length));
            }
        }

        private static void Process(IEnumerable<Param> options, IList<string> values, bool verbose)
        {
            for (var i = 0; i < values.Count; i++)
            {
                foreach (var option in options)
                {
                    if (verbose && option.Type != Enums.ParamType.None)
                        Terminal.Out(ConsoleColor.DarkBlue, "{0} '{1}'", option.Cmds.Last(), values[i]);

                    switch (option.Type)
                    {
                        case Enums.ParamType.Command:
                        case Enums.ParamType.Trigger:
                            values[i] = option.Method(values[i], null);
                            break;

                        case Enums.ParamType.Crypto:
                            var key = Terminal.In("'{0}' {1} key ", values[i], option.Cmds.Last());
                            values[i] = option.Method(values[i], key);
                            break;
                    }
                }
            }
        }
    }
}