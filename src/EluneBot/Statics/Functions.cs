using EluneBot.Enums;
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
        public static IntPtr CastOrUseAtPosition(XYZ position) =>
            Marshal.GetDelegateForFunctionPointer<CastOrUseAtPositionDelegate>(Offsets.Functions.CastOrUseAtPosition).Invoke(position);

        delegate IntPtr GetPointerForGuidDelegate(ulong guid);
        public static IntPtr GetPointerForGuid(ulong guid) =>
            Marshal.GetDelegateForFunctionPointer<GetPointerForGuidDelegate>(Offsets.Functions.GetPointerForGuid).Invoke(guid);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate void ClickToMoveDelegate(IntPtr playerPtr, uint clickType, ref ulong interactGuidPtr, ref XYZ position, float precision);
        public static void ClickToMove(IntPtr playerPtr, uint clickType, ref ulong interactGuidPtr, ref XYZ position, float precision) =>
            Marshal.GetDelegateForFunctionPointer<ClickToMoveDelegate>(Offsets.Functions.ClickToMove).Invoke(playerPtr, clickType, ref interactGuidPtr, ref position, precision);

        [DllImport(Strings.FastCall, EntryPoint = "EnumerateVisibleObjects")]
        public static extern void EnumerateVisibleObjects(IntPtr callback, int filter, IntPtr ptr);

    }
}
