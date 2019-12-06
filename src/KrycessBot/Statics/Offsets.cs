using System;

namespace KrycessBot.Statics
{
    internal static class Offsets
    {
        //public static IntPtr BaseAddress = System.Diagnostics.Process.GetCurrentProcess().MainModule.BaseAddress;

        /*public static class EntityManager
        {
            public static IntPtr Base = (IntPtr)0xCD3BC0;
            public static int First = 0x0;
            public static int Next = 0x0;
            public static int Guid = 0x0;
            public static int Type = 0x0;
        }

        public static class LocalPlayer
        {
            public static IntPtr Base = (IntPtr)0x1744E0;
            public static IntPtr Scale = Functions.GetLocalPlayerBase() + 0x1820;
        }*/

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
    }
}
