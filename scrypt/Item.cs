namespace scrypt
{
    public class Item
    {
        public string Command { get; set; }

        public int Index { get; set; }

        public string Value { get; set; }

        public Item()
        {
        }

        public Item(int index, string command, string value)
        {
            Index = index;
            Command = command.IsCommand() ? command.Replace("/", string.Empty) : null;
            Value = value.IsCommand() || string.IsNullOrEmpty(value) ? null : value;
        }

        public Item(int index, string value)
        {
            Index = index;
            Value = value;
        }

        public Item(int index, char character)
        {
            Index = index;
            Value = character.ToString();
        }

        public override string ToString()
        {
            return string.Join(" ", Index, Command, Value);
        }
    }
}