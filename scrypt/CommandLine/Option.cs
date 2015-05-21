using scrypt.Utils;

namespace scrypt.CommandLine
{
    public class Option
    {
        public string[] Cmds { get; set; }

        public string Help { get; set; }

        public short Order { get; set; }

        public Enums.ParamType Type { get; set; }

        public Option(string[] cmds, string help, Enums.ParamType type)
        {
            Cmds = cmds;
            Help = help;
            Type = type;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}", Cmds.String(" ").PadRight(10), Help);
        }
    }
}