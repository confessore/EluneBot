using EluneBot.Enums;
using EluneBot.Statics;
using System;

namespace EluneBot.Models
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }

        public int Health =>
            GetDescriptor<int>(Offsets.Descriptors.Health);

        public int MaxHealth =>
            GetDescriptor<int>(Offsets.Descriptors.MaxHealth);
    }
}
