using EluneBot.Services;
using System;

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
            return value == IntPtr.Zero ? IntPtr.Zero : MemoryService.ProcessSharp.Memory.Read<IntPtr>(value);
        }

        internal static IntPtr PointsTo(this int value)
        {
            return value == 0 ? IntPtr.Zero : MemoryService.ProcessSharp.Memory.Read<IntPtr>((IntPtr)value);
        }

        internal static IntPtr PointsTo(this uint value)
        {
            return value == 0 ? IntPtr.Zero : MemoryService.ProcessSharp.Memory.Read<IntPtr>((IntPtr)value);
        }

        internal static T ReadAs<T>(this IntPtr value) where T : struct
        {
            return value == IntPtr.Zero ? default(T) : MemoryService.ProcessSharp.Memory.Read<T>(value);
        }

        internal static T ReadAs<T>(this int value) where T : struct
        {
            return value == 0 ? default(T) : MemoryService.ProcessSharp.Memory.Read<T>((IntPtr)value);
        }

        internal static T ReadAs<T>(this uint value) where T : struct
        {
            return value == 0 ? default(T) : MemoryService.ProcessSharp.Memory.Read<T>((IntPtr)value);
        }
    }
}
