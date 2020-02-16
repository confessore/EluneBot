using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EluneBot.Services
{
    public sealed class EndSceneService : IEndSceneService
    {
        public EndSceneService()
        {
            ThrottleFPS();
        }

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        int lastFrameTick;
        int timeBetweenFrame;
        int waitTilNextFrame;

        Direct3D9EndScene endSceneOriginal;
        IntPtr endScenePtr;
        Direct3D9ISceneEnd iSceneEndDelegate;
        IntPtr target;
        List<byte> original;

        // if frames are rendering faster than once every ~16ms (60fps), slow them down
        // this corrects an issue where ClickToMove doesn't work when your monitor has a refresh rate above ~80
        internal void ThrottleFPS()
        {
            GetEndScenePtr();
            endSceneOriginal = Marshal.GetDelegateForFunctionPointer<Direct3D9EndScene>(MemoryService.ProcessSharp.Memory.Read<IntPtr>(endScenePtr));
            var endSceneDetour = new Direct3D9EndScene(EndSceneHook);

            var addrToDetour = Marshal.GetFunctionPointerForDelegate(endSceneDetour);
            var customBytes = BitConverter.GetBytes((int)addrToDetour);
            MemoryService.ProcessSharp.Memory.Write(endScenePtr, customBytes);
        }

        int EndSceneHook(IntPtr parDevice)
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

            return endSceneOriginal(parDevice);
        }

        void GetEndScenePtr()
        {
            iSceneEndDelegate = Marshal.GetDelegateForFunctionPointer<Direct3D9ISceneEnd>(Offsets.Functions.IsSceneEnd);
            target = Marshal.GetFunctionPointerForDelegate(iSceneEndDelegate);
            var hook = Marshal.GetFunctionPointerForDelegate(new Direct3D9ISceneEnd(ISceneEndHook));

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
                Task.Delay(3);
        }

        IntPtr ISceneEndHook(IntPtr ptr)
        {
            var ptr1 = MemoryService.ProcessSharp.Memory.Read<IntPtr>(IntPtr.Add(ptr, 0x38A8));
            var ptr2 = MemoryService.ProcessSharp.Memory.Read<IntPtr>(ptr1);
            endScenePtr = IntPtr.Add(ptr2, 0xa8);

            // unhook ISceneEnd
            MemoryService.ProcessSharp.Memory.Write(target, original.ToArray());

            return iSceneEndDelegate(ptr);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9EndScene(IntPtr device);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr Direct3D9ISceneEnd(IntPtr unk);
    }
}
