using scrypt.CommandLine;
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

            if (args.Contains("/help"))
            {
                Options.List.ForEach(Console.WriteLine);
                return;
            }

            try
            {
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