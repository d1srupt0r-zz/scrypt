namespace scrypt.CommandLine
{
    public class Item
    {
        public int Index { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}