using EluneBot.Services;
using System;
using System.Text;

namespace EluneBot.Extensions
{
    internal static class MemoryExtensions
    {
        public static IntPtr Add(this IntPtr value, IntPtr toAdd)
        {
            return IntPtr.Add(value, (int)toAdd);
        }

        public static IntPtr Add(this IntPtr value, int toAdd)
        {
            return IntPtr.Add(value, toAdd);
        }

        public static IntPtr PointsTo(this IntPtr value)
        {
            return value == IntPtr.Zero ? IntPtr.Zero : App.ProcessSharp.Memory.Read<IntPtr>(value);
        }

        internal static IntPtr PointsTo(this int value)
        {
            return value == 0 ? IntPtr.Zero : App.ProcessSharp.Memory.Read<IntPtr>((IntPtr)value);
        }

        internal static IntPtr PointsTo(this uint value)
        {
            return value == 0 ? IntPtr.Zero : App.ProcessSharp.Memory.Read<IntPtr>((IntPtr)value);
        }

        internal static T ReadAs<T>(this IntPtr value) where T : struct
        {
            return value == IntPtr.Zero ? default : App.ProcessSharp.Memory.Read<T>(value);
        }

        internal static T ReadAs<T>(this int value) where T : struct
        {
            return value == 0 ? default : App.ProcessSharp.Memory.Read<T>((IntPtr)value);
        }

        internal static T ReadAs<T>(this uint value) where T : struct
        {
            return value == 0 ? default : App.ProcessSharp.Memory.Read<T>((IntPtr)value);
        }

        internal static string ReadString(this IntPtr value, int length = 512, Encoding encoding = null)
        {
            if (value == IntPtr.Zero) return "";
            if ((int)value < 00401000) return "";
            if (encoding == null)
                encoding = Encoding.ASCII;
            try
            {
                return App.ProcessSharp.Memory.Read((IntPtr)value, encoding, length);
            }
            catch (Exception)
            {
                return "";
            }
        }

        internal static string ReadString(this int value, int length = 512, Encoding encoding = null)
        {
            if (value == 0) return "";
            if (value < 00401000) return "";
            if (encoding == null)
                encoding = Encoding.ASCII;
            try
            {
                return App.ProcessSharp.Memory.Read((IntPtr)value, encoding, length);
            }
            catch (Exception)
            {
                return "";
            }
        }

        internal static string ReadString(this uint value, int length = 512, Encoding encoding = null)
        {
            if (value == 0) return "";
            if ((int)value < 00401000) return "";
            if (encoding == null)
                encoding = Encoding.ASCII;
            try
            {
                return App.ProcessSharp.Memory.Read((IntPtr)value, encoding, length);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
