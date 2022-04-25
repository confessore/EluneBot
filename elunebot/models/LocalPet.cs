using elunebot.models.enums;
using System;

namespace elunebot.models
{
    public class LocalPet : WoWUnit
    {
        public LocalPet(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
