using scrypt.CommandLine;
using scrypt.Utils;
using System;
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

                var input = Console.IsInputRedirected ? Console.ReadLine() : args.FirstOrDefault(arg => !arg.StartsWith("/"));
                string key = null;

                if (args.Contains("/c") || args.Contains("/h"))
                {
                    Console.Write("Key (enter for Z:W): ");
                    var k = Console.ReadLine();
                    key = string.IsNullOrEmpty(k) ? "Z:W" : k;
                }

                var cmdlist = Options.List.Where(option => option.Cmds.Intersect(args).Any());
                cmdlist.ToList().ForEach(x => Console.WriteLine("{0}", x.Method(input, key)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
#if DEBUG
                Console.Error.WriteLine(e.StackTrace);
#endif
            }
        }
    }
}