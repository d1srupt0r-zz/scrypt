using System.Linq;

namespace scrypt.Utils
{
    public class Const
    {
        public const string AliasPrefix = @"[#!]";
        public const string Alphabet = @"abcdefghijklmnopqrstuvwxyz";
        public const string CommandPrefix = @"[-/]";
        public const string Example = @"TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4=";

        public static string GetValue(string value)
        {
            var name = AliasPrefix.ToRegex().Replace(value, string.Empty);
            var @const = new Const().GetType().GetFields();
            var field = @const.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            return field != null ? field.GetRawConstantValue().ToString() : string.Empty;
        }
    }
}