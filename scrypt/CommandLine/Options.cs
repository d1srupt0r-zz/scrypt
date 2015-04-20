using scrypt.Utils;
using System.Collections.Generic;

namespace scrypt.CommandLine
{
    public class Options
    {
        public static List<Param> List = new List<Param> {
            new Param(new [] { "/help" }, "Display [h]elp")
            ,new Param(new [] { "/v", "/verbose" }, "Display [v]erbose output")
            ,new Param(0, new [] { "/e", "/encode" }, (x, k) => x.Encode(), "Base64 [e]ncode text")
            ,new Param(1, new [] { "/d", "/decode" }, (x, k) => x.Decode(), "Base64 [d]ecode text")
            ,new Param(2, new [] { "/f", "/flip" }, (x, k) => x.Flip(), "Execute character [f]lip on text")
            ,new Param(3, new [] { "/t", "/twist" }, (x, k) => x.Twist(), "Run [t]wist on text (alternating case)")
            ,new Param(4, new [] { "/c", "/cipher" }, (x, k) => x.Cipher(k), "De[c]ipher text using a [k]ey (default Z:W)")
            ,new Param(5, new [] { "/h", "/hash" }, (x, k) => x.Hash(k), "Execute [h]ash algorithm on text (default sha1)")
        };
    }
}