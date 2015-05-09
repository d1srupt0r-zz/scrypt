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
            else if (args.Exists(Const.CommandPrefix, "help"))
                Options.List.ForEach(o => Terminal.Out(ConsoleColor.Yellow, o.ToString()));
            else if (args.Exists(Const.CommandPrefix, "#", "!"))
                Const.GetAll().ForEach(f => Terminal.Out(ConsoleColor.DarkGreen, "{0}{1}\t{2}",
                    "#", f.Name.ToLower(), f.GetRawConstantValue().Limit()));
            else
            {
                try { Process(args); }
                catch (Exception e)
                {
                    Terminal.Out(ConsoleColor.Red, e.Message);
#if DEBUG
                    Terminal.Out(ConsoleColor.DarkGray, e.StackTrace);
#endif
                }
            }
        }

        private static void Output(IEnumerable<string> values, bool verbose)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                    Terminal.Out(ConsoleColor.Green, verbose ? "{0} : {1}" : "{0}", value, value.Length);
            }
        }

        private static void Process(string[] args)
        {
            var verbose = args.Exists(Const.CommandPrefix, "verbose", "v");
            var options = Options.GetAll(args);
            var junk = Combine(args);
            ExecuteAll(options, junk, verbose);
            Output(junk, verbose);
        }

        private static IList<string> Combine(params string[] values)
        {
            return values.String()
                .Split(values.Parse(Const.CommandPrefix + @"\S+"), StringSplitOptions.RemoveEmptyEntries)
                .ReplaceAll(Const.AliasPrefix, Const.GetValue)
                .ToList()
                .Append(Console.IsInputRedirected ? Console.In.ReadToEnd().Cleanse() : null);
        }

        private static void ExecuteAll(IEnumerable<Param> options, IList<string> values, bool verbose)
        {
            for (int i = 0; i < values.Count; i++)
            {
                foreach (var option in options)
                {
                    values[i] = Execute(option, values[i], verbose);
                }
            }
        }

        private static string Execute(Param option, string value, bool verbose)
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

            return value;
        }
    }
}