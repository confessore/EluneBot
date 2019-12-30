using EluneBot.Enums;
using System;

namespace EluneBot.Game.Models
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
