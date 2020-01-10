using EluneBot.Enums;
using System;

namespace EluneBot.Models
{
    public class LocalPet : WoWUnit
    {
        public LocalPet(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }
    }
}
