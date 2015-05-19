using System;
using System.Collections.Generic;
using System.Linq;

namespace scrypt.CommandLine
{
    public class Theme
    {
        public ConsoleColor Alias { get; set; }

        public ConsoleColor Default { get; set; }

        public ConsoleColor Error { get; set; }

        public ConsoleColor Help { get; set; }

        public ConsoleColor Input { get; set; }

        public string Name { get; set; }

        public ConsoleColor Output { get; set; }

        public ConsoleColor Verbose { get; set; }

        public ConsoleColor Warning { get; set; }

        public override string ToString()
        {
            var colors = string.Join(",", new[] { Alias, Default, Error, Help, Input, Output, Verbose, Warning });
            return string.Format("{0}: [{1}]", Name, colors);
        }

        public Theme()
        {
            Name = "Dark";
            Alias = ConsoleColor.DarkGreen;
            Default = ConsoleColor.Blue;
            Error = ConsoleColor.Red;
            Help = ConsoleColor.Yellow;
            Input = ConsoleColor.DarkGray;
            Output = ConsoleColor.Green;
            Verbose = ConsoleColor.DarkBlue;
            Warning = ConsoleColor.DarkMagenta;
        }
    }

    public class Themes
    {
        public static List<Theme> List = new List<Theme> {
            new Theme()
            ,new Theme { Name = "Light", Default = ConsoleColor.DarkCyan, Input = ConsoleColor.Gray, Verbose = ConsoleColor.DarkGray, Warning = ConsoleColor.Magenta }
        };

        public static Theme Find(string name)
        {
            return List.FirstOrDefault(x => x.Name == name);
        }
    }
}