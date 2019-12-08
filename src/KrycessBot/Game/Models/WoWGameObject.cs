using KrycessBot.Enums;
using System;

namespace KrycessBot.Game.Models
{
    public class WoWGameObject : WoWObject
    {
        internal WoWGameObject(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type)
        {
        }
    }
}
