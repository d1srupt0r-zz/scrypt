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
                Console.WriteLine(Const.Help);
                return;
            }

            try
            {
                args.Select((c, i) => new Item
                {
                    Value = c,
                    Index = i,
                    Type = args.Get(i - 1).Type()
                })
                .Where(item => item.Type != Enums.Type.None)
                .ToList()
                .ForEach(item => Console.WriteLine("{0} : {1}", item.ToString(), item.Value.Length));
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