using KrycessBot.Structs;
using System;
using System.Runtime.InteropServices;

namespace KrycessBot.Statics
{
    internal static class Functions
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate IntPtr GetLocalPlayerGuidDelegate();
        public static IntPtr GetLocalPlayerGuid() =>
            Marshal.GetDelegateForFunctionPointer<GetLocalPlayerGuidDelegate>(Offsets.Functions.ClntObjMgrGetActivePlayer).Invoke();

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate IntPtr SelectCharacterDelegate();
        public static IntPtr SelectCharacter() =>
            Marshal.GetDelegateForFunctionPointer<SelectCharacterDelegate>(Offsets.Functions.SelectCharacter).Invoke();

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate IntPtr CastOrUseAtPositionDelegate(ref XYZ position);
        public static IntPtr CastOrUseAtPosition(XYZ xyz) =>
            Marshal.GetDelegateForFunctionPointer<CastOrUseAtPositionDelegate>(Offsets.Functions.CastOrUseAtPosition).Invoke(ref xyz);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate IntPtr GetPointerForGuidDelegate();
        public static IntPtr GetPointerForGuid(long guid) =>
            Marshal.GetDelegateForFunctionPointer<GetPointerForGuidDelegate>(Offsets.Functions.GetPointerForGuid).Invoke();

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate IntPtr EnumerateVisibleObjectsDelegate();
        public static IntPtr EnumerateVisibleObjects(IntPtr callback, int filter) =>
            Marshal.GetDelegateForFunctionPointer<EnumerateVisibleObjectsDelegate>(Offsets.Functions.EnumerateVisibleObjects).Invoke();
    }
}
