using scrypt.Utils;
using System;

namespace scrypt.CommandLine
{
    public class Param
    {
        public string[] Cmds { get; set; }

        public string Help { get; set; }

        public Func<string, string, string> Method { get; set; }

        public string Name { get; set; }

        public Param(string[] cmds, string help)
        {
            Help = help;
            Method = (x, k) => x;
            Cmds = cmds;
        }

        public Param(string[] cmds, Func<string, string, string> method, string help)
        {
            Help = help;
            Method = method;
            Cmds = cmds;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}", Cmds.String(" ").PadRight(10), Help);
        }
    }
}