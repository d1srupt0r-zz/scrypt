using scrypt.CommandLine;
using scrypt.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace scrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var @params = Options.GetAll(args);
            var junk = args.String().Split(
                @params.SelectMany(x => x.Cmds).ToArray(),
                StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Parse(Param param, string value)
        {
            switch (param.Type)
            {
                case Enums.ParamType.Command:
                case Enums.ParamType.Trigger:
                    return param.Method(value, null);
                case Enums.ParamType.Crypto:
                    Console.Write("Key: ");
                    var key = Console.ReadLine();
                    return param.Method(value, key);
            }

            return null;
        }
    }
}