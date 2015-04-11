using System;

namespace scrypt.CommandLine
{
    public class Item
    {
        public Func<Item, string> Convert { get; set; }

        public int Index { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Convert != null ? Convert(this) : this.Value;
        }
    }
}