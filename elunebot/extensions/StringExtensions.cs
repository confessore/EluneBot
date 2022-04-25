using System;
using System.IO;
using System.Linq;

namespace elunebot.extensions
{
    static class StringExtensions
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
            {
                var file = File.ReadAllBytes(value);
                return file.SequenceEqual(bytes);
            }
            return false;
        }
    }
}
