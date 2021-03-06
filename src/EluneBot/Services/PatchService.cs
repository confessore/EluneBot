﻿using EluneBot.Services.Interfaces;
using EluneBot.Statics;

namespace EluneBot.Services
{
    internal sealed class PatchService : IPatchService
    {
        public PatchService()
        {
            App.ProcessSharp.Memory.Write(Offsets.Patches.CtmPatch, new byte[] { 0x00, 0x00, 0x00, 0x00 });
            App.ProcessSharp.Memory.Write(Offsets.Patches.LuaUnlock, new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 });
        }
    }
}
