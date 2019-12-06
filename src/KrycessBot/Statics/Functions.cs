using System;
using System.Runtime.InteropServices;

namespace KrycessBot.Statics
{
    internal static class Functions
    {
        delegate IntPtr GetLocalPlayerGuidDelegate();
        public static IntPtr GetLocalPlayerGuid() =>
            Marshal.GetDelegateForFunctionPointer<GetLocalPlayerGuidDelegate>(Offsets.Functions.ClntObjMgrGetActivePlayer).Invoke();

        /*delegate IntPtr GetLocalPlayerBaseDelegate();
        public static IntPtr GetLocalPlayerBase() =>
            Marshal.GetDelegateForFunctionPointer<GetLocalPlayerBaseDelegate>(Offsets.LocalPlayer.Base).Invoke();

        delegate IntPtr GetEntityManagerBaseDelegate();
        public static IntPtr GetEntityManagerBase() =>
            Marshal.GetDelegateForFunctionPointer<GetEntityManagerBaseDelegate>(Offsets.EntityManager.Base).Invoke();*/
    }
}
