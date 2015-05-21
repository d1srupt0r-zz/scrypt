using System;
using System.Collections.Generic;
using System.Linq;
using scrypt.CommandLine;
using scrypt.Utils;

namespace scrypt
{
    public class Program
    {
        private static Options po;

        public static void Main(string[] args)
        {
            try { Process(args); }
            catch (Exception e)
            {
                Terminal.Out(Terminal.Theme.Error, e.Message);
#if DEBUG
                Terminal.Out(Terminal.Theme.Input, e.StackTrace);
#endif
            }
        }

        public static void Init()
        {
            // TODO: set all options here?
            po = new Options
            {
                Verbose = false
            };
        }

        private static void Output(IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                    Terminal.Out(Terminal.Theme.Output, po.Verbose ? "{0} : {1}" : "{0}", value, value.Length);
            }
        }

        private static void Process(string[] args)
        {
            Init();
            var options = po.GetAll(args);
            var junk = Combine(args);
            ExecuteAll(options, junk);
            Output(junk);
        }

        private static void Triggers(IEnumerable<Param> options)
        {
            var t = options.Where(o => o.Type == Enums.ParamType.None || o.Type == Enums.ParamType.Trigger);
            po.Verbose = t.Exists("/verbose");
        }

        private static IList<string> Combine(params string[] values)
        {
            return values.String()
                .Split(values.Parse(Const.CommandPrefix + @"\S+"), StringSplitOptions.RemoveEmptyEntries)
                .ReplaceAll(Const.AliasPrefix, Const.GetValue)
                .ToList()
                .Append(Console.IsInputRedirected ? Console.In.ReadToEnd().Cleanse() : null);
        }

        private static void ExecuteAll(IEnumerable<Param> options, IList<string> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                foreach (var option in options)
                {
                    if (option is Param)
                        values[i] = Execute(option as Param, values[i]);
                }
            }
        }

        private static string Execute(Param option, string value)
        {
            switch (option.Type)
            {
                case Enums.ParamType.Command:
                    return po.Verbose
                        ? option.Verbose(value, null)
                        : option.Method(value, null);

                case Enums.ParamType.Crypto:
                    var key = Terminal.In("'{0}' {1} key ", value.Limit(), option.Cmds.Last());
                    return po.Verbose
                        ? option.Verbose(value, key)
                        : option.Method(value, key);
            }

            return value;
        }
    }
}