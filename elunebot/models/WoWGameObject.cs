using elunebot.models.enums;
using System;

namespace elunebot.models
{
    public class WoWGameObject : WoWObject
    {
        internal WoWGameObject(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type)
        {
        }
    }
}
