namespace scrypt
{
    public class Item
    {
        private bool Hide { get; set; }

        public char Character { get; set; }

        public string Command { get; set; }

        public int Index { get; set; }

        public string Value { get; set; }

        public Item()
        {
        }

        public Item(int index, string command, string value)
        {
            Hide = !command.IsCommand() || value.IsCommand();
            Index = index;
            Command = command.IsCommand() ? command.Replace("/", string.Empty) : null;
            Value = value.IsCommand() || string.IsNullOrEmpty(value) ? null : value;
        }

        public Item(int index, char character)
        {
            Index = index;
            Character = character;
        }

        public override string ToString()
        {
            return Hide ? string.Empty : string.Join(" ", new string[] { Index.ToString(), Command, Value });
        }
    }
}