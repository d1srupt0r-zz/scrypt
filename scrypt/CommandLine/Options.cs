using System;
using System.Collections.Generic;
using System.Linq;
using scrypt.Utils;

namespace scrypt.CommandLine
{
    public class Options
    {
        public static List<Param> List = new List<Param> {
            new Param(new [] { "/help" }, "Display [h]elp")
            ,new Param(new [] { "/#", "/!" }, "Display list of aliases")
            ,new Param(new [] { "/v", "/verbose" }, "Display [v]erbose output")
            ,new Param(1, new [] { "/e", "/encode" }, (x, k) => x.Encode(), "Base64 [e]ncode text", Enums.ParamType.Command)
            ,new Param(2, new [] { "/d", "/decode" }, (x, k) => x.Decode(), "Base64 [d]ecode text", Enums.ParamType.Command)
            ,new Param(3, new [] { "/f", "/flip" }, (x, k) => x.Flip(), "Execute character [f]lip on text", Enums.ParamType.Trigger)
            ,new Param(4, new [] { "/t", "/twist" }, (x, k) => x.Twist(), "Run [t]wist on text (alternating case)", Enums.ParamType.Trigger)
            ,new Param(5, new [] { "/c", "/cipher" }, (x, k) => x.Cipher(k), "Run [c]ipher on text using a [k]ey (default Z:W)", Enums.ParamType.Crypto)
            ,new Param(6, new [] { "/x", "/hex" }, (x, k) => x.Hex(), "He[x] encode or decode text", Enums.ParamType.Command)
            ,new Param(7, new [] { "/h", "/hash" }, (x, k) => x.Hash(k), "Execute [h]ash algorithm on text (default sha1)", Enums.ParamType.Crypto)
        };

        public static IEnumerable<Param> GetAll(params string[] args)
        {
            return args.ReplaceAll(@"(--|-)", "/")
                .SelectMany(value => List.Where(param => param.Cmds.Contains(value)))
                .OrderBy(o => o.Order);
        }
    }
}