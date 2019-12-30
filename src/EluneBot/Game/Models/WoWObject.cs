using EluneBot.Enums;
using EluneBot.Models;
using System;

namespace EluneBot.Game.Models
{
    public class WoWObject
    {
        public WoWObject(ulong guid, IntPtr pointer, WoWObjectType type)
        {
            Guid = guid;
            Pointer = pointer;
            Type = type;
        }

        public ulong Guid { get; internal  set; }
        public IntPtr Pointer { get; internal set; }
        public WoWObjectType Type { get; internal set; }
        public bool CanRemove { get; internal set; }
        public string Name { get; internal set; }
        public Location Position { get; internal set; }
    }
}
