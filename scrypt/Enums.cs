using System;

namespace scrypt
{
    public static class Enums
    {
        public enum FormatType
        {
            Encode,
            Decode,
            Cipher,
            Twist
        }

        public enum Orientation
        {
            Scramble,
            Flip,
            Rot
        }

        public static T GetEnumValue<T>(string value)
        {
            return Enum.IsDefined(typeof(T), value) ? (T)Enum.Parse(typeof(T), value) : default(T);
        }
    }
}