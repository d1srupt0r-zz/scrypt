namespace scrypt
{
    public class Item
    {
        public int Index { get; set; }

        public Enums.FormatType Type { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            switch (Type)
            {
                case Enums.FormatType.Encode:
                    return Value.Encode();

                case Enums.FormatType.Decode:
                    return Value.Decode();

                case Enums.FormatType.Cipher:
                    return string.Empty;

                case Enums.FormatType.Twist:
                    return Value.Twist();

                default:
                    return string.Format("{0} {1}", Index, Value);
            }
        }
    }
}