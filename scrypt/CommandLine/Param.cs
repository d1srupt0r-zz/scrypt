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

        public Param(string[] cmds, string help, Enums.ParamType type = Enums.ParamType.None)
        {
            Cmds = cmds;
            Help = help;
            Type = type;
        }

        public Param(short order, string[] cmds, Func<string, string, string> method, string help, Enums.ParamType type)
            : this(cmds, help, type)
        {
            Method = method;
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
            Terminal.Out(Terminal.Theme.Verbose, "{0} '{1}' ({2})", Cmds[Cmds.Length - 1], x.Limit(), timer.Elapsed.Ignore('0', ':'));
            return result;
        }
    }
}