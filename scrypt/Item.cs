namespace scrypt
{
    public class Item
    {
        public int Index { get; set; }

        public Enums.Type Type { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            switch (Type)
            {
                case Enums.Type.Encode:
                    return Value.Encode();

                case Enums.Type.Decode:
                    return Value.Decode();

                case Enums.Type.Cipher:
                    return Value.Cipher();

                case Enums.Type.Twist:
                    return Value.Twist();

                case Enums.Type.Flip:
                    return Value.Flip();

                default:
                    return string.Format("{0} {1}", Index, Value);
            }
        }
    }
}