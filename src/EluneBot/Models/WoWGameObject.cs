using EluneBot.Enums;
using System;

namespace EluneBot.Models
{
    public class WoWGameObject : WoWObject
    {
        internal WoWGameObject(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type)
        {
        }
    }
}
