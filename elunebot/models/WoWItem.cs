using elunebot.models.enums;
using System;

namespace elunebot.models
{
    public class WoWItem : WoWObject
    {
        public WoWItem(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
