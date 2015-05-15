using System;
using scrypt.Utils;

namespace scrypt.CommandLine
{
    public class Param
    {
        public string[] Cmds { get; set; }

        public string Help { get; set; }

        public Func<string, string, string> Method { get; set; }

        public short Order { get; set; }

        public Enums.ParamType Type { get; set; }

        public Param(string[] cmds, string help)
        {
            Cmds = cmds;
            Method = (x, k) => x;
            Help = help;
        }

        public Param(short order, string[] cmds, Func<string, string, string> method, string help, Enums.ParamType type)
        {
            Order = order;
            Cmds = cmds;
            Method = method;
            Help = help;
            Type = type;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}", Cmds.String(" ").PadRight(10), Help);
        }

        public string Verbose(string x, string k)
        {
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            var result = Method.Invoke(x, k);
            timer.Stop();
            Terminal.Out(ConsoleColor.DarkBlue, "{0} '{1}' ({2})", Cmds[Cmds.Length - 1], x.Limit(), timer.Elapsed.Ignore('0', ':'));
            return result;
        }
    }
}