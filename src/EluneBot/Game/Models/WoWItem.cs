using EluneBot.Enums;
using System;

namespace EluneBot.Game.Models
{
    public class WoWItem : WoWObject
    {
        public WoWItem(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
