using System;
using System.Collections.Generic;
using scrypt.Utils;

namespace scrypt.CommandLine
{
    public class Terminal
    {
        public static ConsoleColor RandomColor
        {
            get
            {
                var r = new Random(Environment.TickCount).Next(14) + 1;
                return ((ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)))[r];
            }
        }

        public static Theme Theme
        {
            get
            {
                return Console.BackgroundColor == ConsoleColor.Black
                  ? Themes.Find("Dark")
                  : Themes.Find("Light");
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

        public static void Out(Theme value)
        {
            value.Colors().ForEach(x => Out(x, "{0}", (int)x));
        }

        public static void Out(ConsoleColor color, List<System.Reflection.FieldInfo> list)
        {
            list.ForEach(x => Out(color, "{0}{1}\t{2}", "#", x.Name.ToLower(), x.GetRawConstantValue().Limit()));
        }

        public static void Out<T>(ConsoleColor color, List<T> list)
        {
            list.ForEach(x => Out(color, "{0}", x.ToString()));
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