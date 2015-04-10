using System;
using System.Collections.Generic;

namespace scrypt.CommandLine
{
    public class Options
    {
        public static List<Param> List = new List<Param> {
            new Param("e, encode", (x, k) => x.Encode(), "Base64 [e]ncode text"),
            new Param("d, decode", (x, k) => x.Decode(), "Base64 [d]ecode text"),
            new Param("c, cipher", (x, k) => x.Cipher(k), "De[c]ipher text using a [k]ey"),
            new Param("h, hash", (x, k) => x.Hash(k), "Execute [h]ash algorithm on text (default sha1)")
        };
    }

    public class Param
    {
        private string _cmds;
        private string _help;
        private Func<string, string, string> _method;

        public Param(string cmds, Func<string, string, string> method, string help)
        {
            _help = help;
            _method = method;
            _cmds = cmds;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}", _cmds.PadRight(10), _help);
        }
    }
}