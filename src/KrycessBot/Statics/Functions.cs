using KrycessBot.Structs;
using System;
using System.Runtime.InteropServices;

namespace KrycessBot.Statics
{
    internal static class Functions
    {
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

        delegate int EnumerateVisibleObjectsDelegate(IntPtr callback, int filter);
        public static int EnumerateVisibleObjects(IntPtr callback, int filter) =>
            Marshal.GetDelegateForFunctionPointer<EnumerateVisibleObjectsDelegate>(Offsets.Functions.EnumerateVisibleObjects).Invoke(callback, filter);
    }
}
