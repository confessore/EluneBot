using elunebot.models.structs;
using System;
using System.Runtime.InteropServices;

namespace elunebot.statics
{
    static class Functions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr GetLocalPlayerGuidDelegate();
        public static IntPtr GetLocalPlayerGuid() =>
            Marshal.GetDelegateForFunctionPointer<GetLocalPlayerGuidDelegate>(Offsets.Functions.ClntObjMgrGetActivePlayer).Invoke();

        delegate IntPtr GetPointerForGuidDelegate(ulong guid);
        public static IntPtr GetPointerForGuid(ulong guid) =>
            Marshal.GetDelegateForFunctionPointer<GetPointerForGuidDelegate>(Offsets.Functions.GetPointerForGuid).Invoke(guid);

        [DllImport(Strings.FastCall, EntryPoint = "EnumerateVisibleObjects")]
        public static extern void EnumerateVisibleObjects(IntPtr callback, int filter, IntPtr pointer);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate void ClickToMoveDelegate(IntPtr playerPtr, uint clickType, ref ulong interactGuidPtr, ref XYZ position, float precision);
        public static void ClickToMove(IntPtr playerPtr, uint clickType, ref ulong interactGuidPtr, ref XYZ position, float precision) =>
            Marshal.GetDelegateForFunctionPointer<ClickToMoveDelegate>(Offsets.Functions.ClickToMove).Invoke(playerPtr, clickType, ref interactGuidPtr, ref position, precision);
        
        [DllImport(Strings.FastCall, EntryPoint = "DoString", CallingConvention = CallingConvention.StdCall)]
        public static extern void DoString(string luaCode, IntPtr pointer);
    }
}
