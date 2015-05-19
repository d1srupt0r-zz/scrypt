using System;

namespace scrypt.CommandLine
{
    public class Terminal
    {
        public static Theme Theme
        {
            get
            {
                return Console.BackgroundColor == ConsoleColor.Black
                  ? Themes.Find("Dark")
                  : Themes.Find("Light");
            }
        }

        public static ConsoleColor RandomColor
        {
            get
            {
                var r = new Random(Environment.TickCount).Next(14) + 1;
                return ((ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)))[r];
            }
        }

        public static string In(string format, params object[] args)
        {
            if (!Console.IsInputRedirected)
            {
                Out(Theme.Input, false, format, args);
                return Console.ReadLine();
            }

            return null;
        }

        public static void Out(ConsoleColor color, string format, params object[] args)
        {
            Out(color, true, format, args);
        }

        public static void Out(ConsoleColor color, bool writeLine, string format, params object[] args)
        {
            Console.ForegroundColor = color;

            if (writeLine)
                Console.WriteLine(format, args);
            else
                Console.Write(format, args);

            Console.ResetColor();
        }
    }
}