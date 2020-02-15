using EluneBot.Enums;
using EluneBot.Extensions;
using EluneBot.Services;
using EluneBot.Statics;
using System;

namespace EluneBot.Models
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(ulong guid, IntPtr pointer, WoWObjectType type)
            : base(guid, pointer, type) { }

        public override string Name
        {
            get
            {
                try
                {
                    switch (Type)
                    {
                        case WoWObjectType.OT_UNIT:
                            return UnitName;
                        case WoWObjectType.OT_PLAYER:
                            return PlayerName;
                    }
                }
                catch { }
                return "";
            }
        }

        string PlayerName
        {
            get
            {
                var nameBasePtr = Offsets.PlayerObject.NameBase.ReadAs<IntPtr>();
                while (true)
                {
                    var nextGuid = IntPtr.Add(nameBasePtr, Offsets.PlayerObject.NameBaseNextGuid).ReadAs<ulong>();
                    if (nextGuid == 0)
                        return "";
                    if (nextGuid != Guid)
                        nameBasePtr = nameBasePtr.ReadAs<IntPtr>();
                    else
                        break;
                }
                return nameBasePtr.Add(Offsets.PlayerObject.PlayerNameOffset).ReadString();
            }
        }

        string UnitName
        {
            get
            {
                var ptr1 = ReadRelative<IntPtr>(Offsets.Unit.NameBase);
                var ptr2 = ptr1.ReadAs<IntPtr>();
                return ptr2.ReadString();
            }
        }

        public int Health =>
            GetDescriptor<int>(Offsets.Descriptors.Health);

        public int MaxHealth =>
            GetDescriptor<int>(Offsets.Descriptors.MaxHealth);

        public int Mana =>
            GetDescriptor<int>(Offsets.Descriptors.Mana);

        public int MaxMana =>
            GetDescriptor<int>(Offsets.Descriptors.MaxMana);

        public override Location Position
        {
            get
            {
                try
                {
                    var x = ReadRelative<float>(Offsets.Unit.PosX);
                    var y = ReadRelative<float>(Offsets.Unit.PosY);
                    var z = ReadRelative<float>(Offsets.Unit.PosZ);
                    return new Location(x, y, z);
                }
                catch { return new Location(0, 0, 0); }
            }
        }
    }
}
