using KrycessBot.Enums;
using System;

namespace KrycessBot.Game.Models
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(long guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
