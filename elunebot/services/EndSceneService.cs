using elunebot.extensions;
using elunebot.services.interfaces;
using elunebot.statics;
using GreyMagic;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace elunebot.services
{
    sealed class EndSceneService //: IEndSceneService
    {
        //readonly InProcessMemoryReader _reader;

        //public EndSceneService(
        //    InProcessMemoryReader reader)
        //{
        //    _reader = reader;
        //}

        //int lastFrameTick;
        //int timeBetweenFrame;
        //int waitTilNextFrame;

        //Direct3D9EndSceneDelegate endSceneOriginal;
        //IntPtr endScenePtr;
        //Direct3D9ISceneEndDelegate iSceneEndDelegate;
        //IntPtr target;
        //List<byte> original;

        //// if frames are rendering faster than once every ~16ms (60fps), slow them down
        //// this corrects an issue where ClickToMove doesn't work when your monitor has a refresh rate above ~80
        //public void ThrottleFPS()
        //{
        //    GetEndScenePtr();
        //    endSceneOriginal = Marshal.GetDelegateForFunctionPointer<Direct3D9EndSceneDelegate>(endScenePtr.ReadAs<IntPtr>(_reader));
        //    var endSceneDetour = new Direct3D9EndSceneDelegate(EndSceneHook);
        //    var addrToDetour = Marshal.GetFunctionPointerForDelegate(endSceneDetour);
        //    var customBytes = BitConverter.GetBytes((int)addrToDetour);
        //    _reader.WriteBytes(endScenePtr.ToString(), customBytes);
        //}

        //int EndSceneHook(IntPtr device)
        //{
        //    try
        //    {
        //        if (lastFrameTick != 0)
        //        {
        //            timeBetweenFrame = Environment.TickCount - lastFrameTick;
        //            if (timeBetweenFrame < 15)
        //            {
        //                var newCount = Environment.TickCount;
        //                waitTilNextFrame = 15 - timeBetweenFrame;
        //                newCount += waitTilNextFrame;
        //                while (Environment.TickCount < newCount) { }
        //            }
        //        }
        //        lastFrameTick = Environment.TickCount;
        //    }
        //    catch { }
        //    return endSceneOriginal(device);
        //}

        //void GetEndScenePtr()
        //{
        //    iSceneEndDelegate = Marshal.GetDelegateForFunctionPointer<Direct3D9ISceneEndDelegate>(Offsets.Functions.ISceneEnd);
        //    target = Marshal.GetFunctionPointerForDelegate(iSceneEndDelegate);
        //    var hook = Marshal.GetFunctionPointerForDelegate(new Direct3D9ISceneEndDelegate(ISceneEndHook));
        //    // note the original bytes so we can unhook ISceneEnd after finding endScenePtr
        //    original = new List<byte>();
        //    original.AddRange(_reader.ReadBytes(target.ToString(), 6));

        //    // hook ISceneEnd
        //    var detour = new List<byte> { 0x68 };
        //    var tmp = BitConverter.GetBytes(hook.ToInt32());
        //    detour.AddRange(tmp);
        //    detour.Add(0xC3);
        //    _reader.WriteBytes(target.ToString(), detour.ToArray());

        //    // wait for ISceneEndHook to set endScenePtr
        //    while (endScenePtr == default)
        //        Task.Delay(5).Wait();
        //}

        //IntPtr ISceneEndHook(IntPtr device)
        //{
        //    var ptr1 = (IntPtr)_reader.ReadInt(IntPtr.Add(device, (int)Offsets.Functions.EndScenePtr1).ToString());
        //    var ptr2 = (IntPtr)_reader.ReadInt(ptr1.ToString());
        //    endScenePtr = IntPtr.Add(ptr2, (int)Offsets.Functions.EndScenePtr2);
        //    // unhook ISceneEnd
        //    _reader.WriteBytes(target.ToString(), original.ToArray());
        //    return iSceneEndDelegate(device);
        //}

        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //delegate int Direct3D9EndSceneDelegate(IntPtr device);

        //[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        //delegate IntPtr Direct3D9ISceneEndDelegate(IntPtr unknown);
    }
}
