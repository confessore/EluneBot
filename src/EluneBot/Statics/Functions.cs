using EluneBot.Structs;
using System;
using System.Runtime.InteropServices;

namespace EluneBot.Statics
{
    internal static class Functions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr GetLocalPlayerGuidDelegate();
        public static IntPtr GetLocalPlayerGuid() =>
            Marshal.GetDelegateForFunctionPointer<GetLocalPlayerGuidDelegate>(Offsets.Functions.ClntObjMgrGetActivePlayer).Invoke();

        delegate IntPtr SelectCharacterDelegate();
        public static IntPtr SelectCharacter() =>
            Marshal.GetDelegateForFunctionPointer<SelectCharacterDelegate>(Offsets.Functions.SelectCharacter).Invoke();

        delegate IntPtr CastOrUseAtPositionDelegate(XYZ position);
        public static IntPtr CastOrUseAtPosition(XYZ xyz) =>
            Marshal.GetDelegateForFunctionPointer<CastOrUseAtPositionDelegate>(Offsets.Functions.CastOrUseAtPosition).Invoke(xyz);

        delegate IntPtr GetPointerForGuidDelegate(ulong guid);
        public static IntPtr GetPointerForGuid(ulong guid) =>
            Marshal.GetDelegateForFunctionPointer<GetPointerForGuidDelegate>(Offsets.Functions.GetPointerForGuid).Invoke(guid);

        [DllImport(Strings.FastCall, EntryPoint = "EnumerateVisibleObjects")]
        public static extern void EnumerateVisibleObjects(IntPtr callback, int filter, IntPtr ptr);
        
    }
}
