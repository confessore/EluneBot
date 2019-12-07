using KrycessBot.Enums;
using System;

namespace KrycessBot.Game.Models
{
    public class LocalPlayer : WoWUnit
    {
        public LocalPlayer(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
