using System;
using scrypt.Utils;

namespace scrypt.CommandLine
{
    public class Param : Option
    {
        public Func<string, string, string> Method { get; set; }

        public Param(short order, string[] cmds, Func<string, string, string> method, string help, Enums.ParamType type)
        {
            // base
            Order = order;
            Cmds = cmds;
            Help = help;
            Type = type;
            // new
            Method = method;
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