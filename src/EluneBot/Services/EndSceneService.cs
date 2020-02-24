using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EluneBot.Services
{
    internal sealed class EndSceneService : IEndSceneService
    {
        public EndSceneService()
        {
            //ThrottleFPS();
        }

        int lastFrameTick;
        int timeBetweenFrame;
        int waitTilNextFrame;

        Direct3D9EndSceneDelegate endSceneOriginal;
        IntPtr endScenePtr;
        Direct3D9ISceneEndDelegate iSceneEndDelegate;
        IntPtr target;
        List<byte> original;

        // if frames are rendering faster than once every ~16ms (60fps), slow them down
        // this corrects an issue where ClickToMove doesn't work when your monitor has a refresh rate above ~80
        void ThrottleFPS()
        {
            GetEndScenePtr();
            endSceneOriginal = Marshal.GetDelegateForFunctionPointer<Direct3D9EndSceneDelegate>(MemoryService.ProcessSharp.Memory.Read<IntPtr>(endScenePtr));
            var endSceneDetour = new Direct3D9EndSceneDelegate(EndSceneHook);
            var addrToDetour = Marshal.GetFunctionPointerForDelegate(endSceneDetour);
            var customBytes = BitConverter.GetBytes((int)addrToDetour);
            MemoryService.ProcessSharp.Memory.Write(endScenePtr, customBytes);
        }

        int EndSceneHook(IntPtr device)
        {
            try
            {
                if (lastFrameTick != 0)
                {
                    timeBetweenFrame = Environment.TickCount - lastFrameTick;
                    if (timeBetweenFrame < 15)
                    {
                        var newCount = Environment.TickCount;
                        waitTilNextFrame = 15 - timeBetweenFrame;
                        newCount += waitTilNextFrame;
                        while (Environment.TickCount < newCount) { }
                    }
                }
                lastFrameTick = Environment.TickCount;
            }
            catch { }
            return endSceneOriginal(device);
        }

        void GetEndScenePtr()
        {
            iSceneEndDelegate = Marshal.GetDelegateForFunctionPointer<Direct3D9ISceneEndDelegate>(Offsets.Functions.ISceneEnd);
            target = Marshal.GetFunctionPointerForDelegate(iSceneEndDelegate);
            var hook = Marshal.GetFunctionPointerForDelegate(new Direct3D9ISceneEndDelegate(ISceneEndHook));
            // note the original bytes so we can unhook ISceneEnd after finding endScenePtr
            original = new List<byte>();
            original.AddRange(MemoryService.ProcessSharp.Memory.Read<byte>(target, 6));

            // hook ISceneEnd
            var detour = new List<byte> { 0x68 };
            var tmp = BitConverter.GetBytes(hook.ToInt32());
            detour.AddRange(tmp);
            detour.Add(0xC3);
            MemoryService.ProcessSharp.Memory.Write(target, detour.ToArray());

            // wait for ISceneEndHook to set endScenePtr
            while (endScenePtr == default)
                Task.Delay(5).Wait();
        }

        IntPtr ISceneEndHook(IntPtr device)
        {
            var ptr1 = MemoryService.ProcessSharp.Memory.Read<IntPtr>(IntPtr.Add(device, (int)Offsets.Functions.EndScenePtr1));
            var ptr2 = MemoryService.ProcessSharp.Memory.Read<IntPtr>(ptr1);
            endScenePtr = IntPtr.Add(ptr2, (int)Offsets.Functions.EndScenePtr2);
            // unhook ISceneEnd
            MemoryService.ProcessSharp.Memory.Write(target, original.ToArray());
            return iSceneEndDelegate(device);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int Direct3D9EndSceneDelegate(IntPtr device);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate IntPtr Direct3D9ISceneEndDelegate(IntPtr unknown);
    }
}
