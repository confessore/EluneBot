using EluneBot.Extensions;
using EluneBot.Models;
using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using EluneBot.Structs;
using System;
using System.Runtime.InteropServices;

namespace EluneBot.Services
{
    unsafe sealed class NavigationService : INavigationService
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate XYZ* CalculatePathDelegate(
            uint mapId,
            XYZ start,
            XYZ end,
            bool straightPath,
            out int length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void FreePathArrDelegate(XYZ* pathArr);

        static CalculatePathDelegate calculatePath;
        static FreePathArrDelegate freePathArr;

        public NavigationService()
        {
            var navProcPtr = LoadLibrary(Paths.Navigation);

            var calculatePathPtr = GetProcAddress(navProcPtr, "CalculatePath");
            calculatePath = Marshal.GetDelegateForFunctionPointer<CalculatePathDelegate>(calculatePathPtr);

            var freePathPtr = GetProcAddress(navProcPtr, "FreePathArr");
            freePathArr = Marshal.GetDelegateForFunctionPointer<FreePathArrDelegate>(freePathPtr);
        }

        public Location[] CalculateLocation(uint mapId, Location start, Location end, bool straightPath)
        {
            var ret = calculatePath(mapId, LocationExtensions.ToStruct(start), LocationExtensions.ToStruct(end), straightPath, out int length);
            var list = new Location[length];
            for (var i = 0; i < length; i++)
            {
                list[i] = new Location(ret[i]);
            }
            freePathArr(ret);
            return list;
        }
    }
}