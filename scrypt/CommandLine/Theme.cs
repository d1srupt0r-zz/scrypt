using System;
using System.Linq;
using scrypt.Utils;

namespace scrypt.CommandLine
{
    public class Theme
    {
        private static ConsoleColor FixColor(ConsoleColor color)
        {
            var dark = new[] { ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.DarkGray }
                .Contains(Console.BackgroundColor);
            return dark ? color : Enums.GetEnumValue<ConsoleColor>((int)color % 7);
        }

        public static ConsoleColor Alias
        {
            get { return FixColor(ConsoleColor.DarkGreen); }
        }

        public static ConsoleColor Default
        {
            get { return FixColor(ConsoleColor.Blue); }
        }

        public static ConsoleColor Error
        {
            get { return FixColor(ConsoleColor.Red); }
        }

        public static ConsoleColor Help
        {
            get { return FixColor(ConsoleColor.Yellow); }
        }

        public static ConsoleColor Input
        {
            get { return FixColor(ConsoleColor.DarkGray); }
        }

        public static ConsoleColor Output
        {
            get { return FixColor(ConsoleColor.Green); }
        }

        public static ConsoleColor Verbose
        {
            get { return FixColor(ConsoleColor.DarkBlue); }
        }

        public static ConsoleColor Warning
        {
            get { return FixColor(ConsoleColor.DarkRed); }
        }
    }
}