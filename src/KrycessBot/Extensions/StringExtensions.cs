using System.IO;
using System.Linq;

namespace KrycessBot.Extensions
{
    internal static class StringExtensions
    {
        public static void CheckFile(this string value, byte[] bytes, bool exact = false)
        {
            if (exact)
            {
                if (!value.FileEqualTo(bytes))
                    value.CreateFile(bytes);
            }
            else
            {
                if (!File.Exists(value))
                    File.WriteAllBytes(value, bytes);
            }
        }

        public static void CheckDirectory(this string value)
        {
            if (!Directory.Exists(value))
                Directory.CreateDirectory(value);
        }

        static void CreateFile(this string value, byte[] bytes)
        {
            if (!value.FileEqualTo(bytes))
                File.WriteAllBytes(value, bytes);
        }

        static bool FileEqualTo(this string value, byte[] bytes)
        {
            if (File.Exists(value))
                return File.ReadAllBytes(value).SequenceEqual(bytes);
            return false;
        }
    }
}
