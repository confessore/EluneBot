using EluneBot.Enums;
using EluneBot.Services;
using EluneBot.Statics;
using System;

namespace EluneBot.Models
{
    public class LocalPlayer : WoWUnit
    {
        public LocalPlayer(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }

        public uint MapId => App.ProcessSharp.Memory.Read<uint>(
            IntPtr.Add(App.ProcessSharp.Memory.Read<IntPtr>(Offsets.ObjectManager.ManagerBase), 0xCC));
    }
}
