using System;

namespace EluneBot.Statics
{
    internal static class Offsets
    {
        //public static IntPtr BaseAddress = System.Diagnostics.Process.GetCurrentProcess().MainModule.BaseAddress;

        internal static class ObjectManager
        {
            internal static IntPtr CurObjGuid = (IntPtr)0x30;
            internal static IntPtr ManagerBase = (IntPtr)0x00B41414;
            internal static IntPtr PlayerGuid = (IntPtr)0xC0;
            internal static IntPtr FirstObj = (IntPtr)0xAC;
            internal static IntPtr NextObj = (IntPtr)0x3C;
            internal static IntPtr ObjType = (IntPtr)0x14;
            internal static int DescriptorOffset = 0x8;
        }

        internal static class Descriptors
        {
            internal static int GotLoot = 0xB4;

            internal static int SummonedByGuid = 0x30;

            internal static int NpcId = 0xE74;

            internal static int DynamicFlags = 0x23C;
            internal static int Flags = 0xB8;

            internal static int IsChanneling = 0x240;
            internal static int CreatedByGuid = 0x38;
            internal static int GameObjectCreatedByGuid = 0x18;

            internal static int MovementFlags = 0x9E8;

            internal static int Health = 0x58;
            internal static int MaxHealth = 0x70;
            internal static int FactionId = 0x8C;
            internal static int Mana = 0x5C;
            internal static int MaxMana = 0x74;
            internal static int Rage = 0x60;
            internal static int Energy = 0x68;
            internal static int TargetGuid = 0x40;
            internal static int CorpseOwnedBy = 0x18;

            internal static int ItemId = 0xC;
            internal static int ItemDurability = 0xB8;
            internal static int ItemMaxDurability = 0xBC;
            internal static int ItemStackCount = 0x38;

            internal static int Level = 0x88;

            internal static int MountDisplayId = 0x214;

            internal static int ContainerTotalSlots = 0x6c8;
            internal static int CorpseX = 0x24;
            internal static int CorpseY = 0x28;
            internal static int CorpseZ = 0x2c;

            internal static int NextLevelXp = 0xB34;
            internal static int CurrentXp = 0xB30;
        }

        public static class LocalPlayer
        {
            internal static IntPtr NameBase = (IntPtr)0xC0E230;
            internal static int NameBaseNextGuid = 0xC;
            internal static IntPtr Class = (IntPtr)0xC27E81;
            internal static IntPtr IsInGame = (IntPtr)0xB4B424;
            internal static IntPtr IsGhost = (IntPtr)0x835A48;
            internal static IntPtr Name = (IntPtr)0x827D88;
            internal static IntPtr TargetGuid = (IntPtr)0x74E2D8;
            internal static IntPtr IsChanneling = (IntPtr)0x240;
            internal static IntPtr IsCasting = (IntPtr)0xCECA88;
            internal static int ComboPoints1 = 0xE68;
            internal static int ComboPoints2 = 0x1029;
            internal static IntPtr CharacterCount = (IntPtr)0x00B42140;

            internal static IntPtr CorpsePositionX = (IntPtr)0x00B4E284;
            internal static IntPtr CorpsePositionY = (IntPtr)0x00B4E288;
            internal static IntPtr CorpsePositionZ = (IntPtr)0x00B4E28C;

            internal static IntPtr CtmX = (IntPtr)0xC4D890;
            internal static IntPtr CtmY = (IntPtr)0xC4D894;
            internal static IntPtr CtmZ = (IntPtr)0xC4D898;
            internal static IntPtr CtmState = (IntPtr)0xC4D888;

            internal static int MovementStruct = 0x9A8;

            internal static IntPtr IsLooting = (IntPtr)0xB71B48;
            internal static IntPtr IsInCC = (IntPtr)0xB4B3E4;

            internal static int RealZoneText = 0xB4B404;
            internal static IntPtr ContinentText = (IntPtr)0x00C961A0;
            internal static IntPtr MinimapZoneText = (IntPtr)0xB4DA28;
        }

        internal static class Functions
        {
            internal static IntPtr SelectCharacter = (IntPtr)0x472740;
            internal static IntPtr CastOrUseAtPosition = (IntPtr)0x6E60F0;
            internal static IntPtr EnterWorld = (IntPtr)0x0046B500;

            internal static IntPtr GetCreatureRank = (IntPtr)0x00605620;
            internal static IntPtr GetCreatureType = (IntPtr)0x00605570;
            internal static IntPtr GetCreatureFamily = (IntPtr)0x006055E0;

            internal static IntPtr CastSpellByName = (IntPtr)0x004B4AB0;

            internal static IntPtr RetrieveCorpse = (IntPtr)0x0048D260;
            internal static IntPtr RepopMe = (IntPtr)0x005E0AE0;

            internal static IntPtr LootSlotAt = (IntPtr)0x004C2790;
            internal static IntPtr IsTransport = (IntPtr)0x007C61C0;
            internal static IntPtr ConfirmBindLoot = (IntPtr)0x0048DD00;

            internal static IntPtr CanCompleteQuest = (IntPtr)0x4DF580;
            internal static IntPtr CanUseItem = (IntPtr)0x005EA930;

            internal static IntPtr UnitReaction = (IntPtr)0x006061E0;
            internal static IntPtr SetTarget = (IntPtr)0x493540;
            internal static IntPtr LastHardwareAction = (IntPtr)0x00CF0BC8;
            internal static IntPtr AutoLoot = (IntPtr)0x4C1FA0;
            internal static IntPtr ClickToMove = (IntPtr)0x00611130;
            internal static IntPtr AcceptQuest = (IntPtr)0x005EAC10;
            internal static IntPtr CompleteQuest = (IntPtr)0x005EACA0;
            internal static IntPtr GetText = (IntPtr)0x703BF0;
            internal static IntPtr DoString = (IntPtr)0x00704CD0;
            internal static IntPtr GetEndscene = (IntPtr)0x5A17B6;
            internal static IntPtr IsLooting = (IntPtr)0x006126B0;
            internal static IntPtr GetLootSlots = (IntPtr)0x004C2260;
            internal static IntPtr OnRightClickObject = (IntPtr)0x005F8660;
            internal static IntPtr OnRightClickUnit = (IntPtr)0x60BEA0;
            internal static IntPtr SetFacing = (IntPtr)0x007C6F30;
            internal static IntPtr SendMovementPacket = (IntPtr)0x00600A30;
            internal static IntPtr PerformDefaultAction = (IntPtr)0x00481F60;
            internal static IntPtr CGInputControl__GetActive = (IntPtr)0x005143E0;
            internal static IntPtr CGInputControl__SetControlBit = (IntPtr)0x00515090;
            internal static IntPtr EnumerateVisibleObjects = (IntPtr)0x00468380;
            internal static IntPtr BuyVendorItem = (IntPtr)0x005E1E90;
            internal static IntPtr GetPointerForGuid = (IntPtr)0x464870;
            internal static IntPtr ClntObjMgrGetActivePlayer = (IntPtr)0x00468550;
            internal static IntPtr ClntObjMgrGetMapId = (IntPtr)0x00468580;
            internal static IntPtr NetClientSend = (IntPtr)0x005379A0;
            internal static IntPtr GetSpellCooldown = (IntPtr)0x006E13E0;
            internal static IntPtr GetSpellCooldownPtr1 = (IntPtr)0x00CECAEC;
            internal static IntPtr UseItem = (IntPtr)0x005D8D00;
            internal static IntPtr DbQueryCreatureCache = (IntPtr)0x00556AA0;
            internal static IntPtr DbQueryCreatureCachePtr1 = (IntPtr)0x00C0E354;
            internal static IntPtr ClientConnection = (IntPtr)0x005AB490;

            internal static IntPtr SellItem = (IntPtr)0x005E1D50;

            internal static IntPtr LuaGetArgCount = (IntPtr)0x006F3070;
            internal static IntPtr HandleSpellTerrainClick = (IntPtr)0x6E60F0;

            internal static IntPtr DefaultServerLogin = (IntPtr)0x0046D160;

            internal static IntPtr IsSceneEnd = (IntPtr)0x005A17A0;
            internal static IntPtr EndScenePtr1 = (IntPtr)0x38a8;
            internal static IntPtr EndScenePtr2 = (IntPtr)0xa8;

            internal static IntPtr ItemCacheGetRow = (IntPtr)0x0055BA30;
            internal static IntPtr ItemCacheBasePtr = (IntPtr)0x00C0E2A0;

            internal static IntPtr QuestCacheGetRow = (IntPtr)0x00562A40;
            internal static IntPtr QuestCacheBasePtr = (IntPtr)0x00C0E1B0;

            internal static IntPtr LuaRegisterFunc = (IntPtr)0x00704120;
            internal static IntPtr LuaUnregFunc = (IntPtr)0x00704160;
            internal static IntPtr LuaIsString = (IntPtr)0x006F3510;
            internal static IntPtr LuaIsNumber = (IntPtr)0x006F34D0;
            internal static IntPtr LuaToString = (IntPtr)0x006F3690;
            internal static IntPtr LuaToNumber = (IntPtr)0x006F3620;

            internal static IntPtr Intersect = (IntPtr)0x6aa160;

            internal static IntPtr CastSpell = (IntPtr)0x6E5A90;
            internal static IntPtr AbandonQuest = (IntPtr)0x5EAF40;
            internal static IntPtr GetGameObjectPosition = (IntPtr)0x005F9F50;
        }

        internal static class Patches
        {
            internal static IntPtr DisableCollision1 = (IntPtr)0x6ABC5A;
            //internal static IntPtr DisableCollision2 = (IntPtr) 0x006A467B;
            internal static IntPtr DisableCollision3 = (IntPtr)0x006ABF13; // GameObject Collision
            //internal static IntPtr DisableCollision4 = (IntPtr) 0x006AB494;
            internal static IntPtr CtmPatch = (IntPtr)0x860A90;
            internal static IntPtr Wallclimb = (IntPtr)0x0080DFFC;
            internal static IntPtr LootPatch = (IntPtr)0x004C21C0;
            internal static IntPtr LootPatch2 = (IntPtr)0x004C28FF;
            internal static IntPtr LuaUnlock = (IntPtr)0x494A50;
        }

        internal static class PlayerObject
        {
            internal static IntPtr NameBase = (IntPtr)0xC0E230;
            internal static int NameBaseNextGuid = 0xC;
            internal static int PlayerNameOffset = 0x14;
        }

        internal static class Unit
        {
            internal static int PosX = 0x9B8;
            internal static int PosY = 0x9BC;
            internal static int PosZ = 0x9C0;
            internal static int AuraBase = 0xBC;
            internal static int DebuffBase = 0x13C;

            internal static int NameBase = 0xB30;
            internal static int IsCritterOffset = 24;
        }
    }
}
