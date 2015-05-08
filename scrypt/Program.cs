using System;
using System.Collections.Generic;
using System.Linq;
using scrypt.CommandLine;
using scrypt.Utils;

namespace scrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
                Terminal.Out(ConsoleColor.Blue, Const.Example);
            else if (args.Exists(Const.CommandPrefix, "help"))
                Options.List.ForEach(o => Terminal.Out(ConsoleColor.Yellow, o.ToString()));
            else if (args.Exists(Const.CommandPrefix, "#", "!"))
                Const.GetAll().ForEach(f => Terminal.Out(ConsoleColor.DarkGreen, "{0}{1}\t{2}",
                    "#", f.Name.ToLower(), f.GetRawConstantValue().Limit()));
            else
            {
                try { DoWork(args); }
                catch (Exception e)
                {
                    Terminal.Out(ConsoleColor.Red, e.Message);
#if DEBUG
                    Terminal.Out(ConsoleColor.DarkGray, e.StackTrace);
#endif
                }
            }
        }

        private static void DoWork(string[] args)
        {
            var options = Options.GetAll(args);
            var verbose = args.Exists(Const.CommandPrefix, "verbose", "v");
            var junk = Box(args);
            Output(options, junk, verbose);
        }

        private static void Output(IEnumerable<Param> options, IList<string> values, bool verbose)
        {
            values.SelectMany(value => options.Select(option => Process(option, value, verbose)))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList()
                .ForEach(x => Terminal.Out(ConsoleColor.Green, verbose ? "{0} : {1}" : "{0}", x, x.Length));
        }

        private static IList<string> Box(string[] args)
        {
            return args.String()
                .Split(args.Parse(Const.CommandPrefix + @"\S+"), StringSplitOptions.RemoveEmptyEntries)
                .ReplaceAll(Const.AliasPrefix, Const.GetValue)
                .ToList()
                .Append(Console.IsInputRedirected ? Console.In.ReadToEnd().Cleanse() : null);
        }

        private static string Process(Param option, string value, bool verbose)
        {
            if (verbose && option.Type != Enums.ParamType.None)
                Terminal.Out(ConsoleColor.DarkBlue, "{0} '{1}'", option.Cmds.Last(), value.Limit());

            switch (option.Type)
            {
                case Enums.ParamType.Command:
                case Enums.ParamType.Trigger:
                    return option.Method(value, null);

                case Enums.ParamType.Crypto:
                    var key = Terminal.In("'{0}' {1} key ", value.Limit(), option.Cmds.Last());
                    return option.Method(value, key);
            }

            return null;
        }
    }
}