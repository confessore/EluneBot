using EluneBot.Enums;
using System;

namespace EluneBot.Models
{
    public class LocalPlayer : WoWUnit
    {
        public LocalPlayer(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
