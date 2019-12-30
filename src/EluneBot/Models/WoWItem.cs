using EluneBot.Enums;
using System;

namespace EluneBot.Models
{
    public class WoWItem : WoWObject
    {
        public WoWItem(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
