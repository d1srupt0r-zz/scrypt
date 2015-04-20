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

                var text = new List<string>();
                var cmds = new List<string>();
                //args.Split(text, cmds);

                Scrypt(text, cmds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
#if DEBUG
                Console.Error.WriteLine(e.StackTrace);
#endif
            }
        }

        public static void Parse(string[] args)
        {
            var text = new List<string>();
            var cmds = new Stack<Param>();

            foreach (var arg in args)
            {
                if (arg.StartsWith("/"))
                    cmds.Push (Options.List.FirstOrDefault(x => x.Cmds.Contains(arg)));
                else
                    text.Add(arg);
            }
        }

        public static void Scrypt(IList<string> text, IList<string> cmds)
        {
            string key = null;

            var input = string.Empty;

            if (cmds.Contains("/c") || cmds.Contains("/h"))
            {
                Console.Write("Key: ");
                key = Console.ReadLine();
            }

            //cmds.OrderBy(x => x.Order).Select(x => x.Method(input, key)).ToList().ForEach(Console.WriteLine);
        }
    }
}