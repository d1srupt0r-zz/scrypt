using scrypt.CommandLine;
using scrypt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace scrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(Const.Example);
                return;
            }

            try
            {
                if (args.Contains("/help"))
                {
                    Options.List.ForEach(Console.WriteLine);
                    return;
                }

                var @params = Parse<Param>(args);
                //input.SelectMany(x => @params.OrderBy(o => o.Order).Select(o => o.Method(x, null)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
#if DEBUG
                Console.Error.WriteLine(e.StackTrace);
#endif
            }
        }

        public static IEnumerable<T> Parse<T>(string[] args)
        {
            var data = string.Join(" ", args);

            var cmds = @"/\S(?=\s[^/%])".ToRegex().Matches(data);
            var triggers = @"/\S(?=(\s/|$))".ToRegex().Matches(data);
            var text = @"[^-/]\w+".ToRegex().Matches(data);

            return null;
        }

        public static void Scrypt(IEnumerable<Param> @params)
        {
            string key = null;

            var input = string.Empty;

            /*if (@params)
            {
                Console.Write("Key: ");
                key = Console.ReadLine();
            }*/

            //cmds.OrderBy(x => x.Order).Select(x => x.Method(input, key)).ToList().ForEach(Console.WriteLine);
        }
    }
}