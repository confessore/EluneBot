using KrycessBot.Enums;
using System;

namespace KrycessBot.Game.Entities
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
