using EluneBot.Enums;
using System;

namespace EluneBot.Models
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
