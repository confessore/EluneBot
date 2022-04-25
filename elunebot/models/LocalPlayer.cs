using elunebot.models.enums;
using elunebot.statics;
using System;

namespace elunebot.models
{
    public class LocalPlayer : WoWUnit
    {
        public LocalPlayer(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }

        public uint MapId => App.Reader.Read<uint>(
            IntPtr.Add(App.Reader.Read<IntPtr>(Offsets.ObjectManager.ManagerBase), 0xCC));
    }
}
