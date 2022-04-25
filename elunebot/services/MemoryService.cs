using elumebot.models.enums;
using elunebot.extensions;
using elunebot.models;
using elunebot.models.enums;
using elunebot.models.structs;
using elunebot.services.interfaces;
using elunebot.statics;
using System;
using System.Threading.Tasks;

namespace elunebot.services
{
    sealed class MemoryService : IMemoryService
    {
        readonly IMainThreadService _mainThread;

        public MemoryService(
            IMainThreadService mainThread)
        {
            _mainThread = mainThread;
        }

        /// <summary>
        /// checks to see if the player is logged into the game world
        /// </summary>
        /// <returns>bool</returns>
        public bool IsInGame() =>
                Offsets.LocalPlayer.IsInGame.ReadAs<bool>();

        /// <summary>
        /// gets the player's guid
        /// </summary>
        /// <returns>ulong</returns>
        public ulong GetLocalPlayerGuid() =>
                (ulong)Functions.GetLocalPlayerGuid();

        /// <summary>
        /// gets the base of the guid system (object manager)
        /// </summary>
        /// <returns>IntPtr</returns>
        public IntPtr GetPointerForGuid(ulong guid) =>
                Functions.GetPointerForGuid(guid);

        /// <summary>
        /// gets the object type from the pointer to the object
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns>WoWObjectType</returns>
        public WoWObjectType GetWoWObjectType(IntPtr pointer) =>
            (WoWObjectType)App.Reader.Read<byte>(IntPtr.Add(pointer, (int)Offsets.ObjectManager.ObjType));

        /// <summary>
        /// enumerates all visible objects around the player
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="filter"></param>
        public void EnumerateVisibleObjects(IntPtr callback, int filter) =>
            Functions.EnumerateVisibleObjects(callback, filter, Offsets.Functions.EnumerateVisibleObjects);

        /// <summary>
        /// clicks to move the player
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public void ClickToMove(Location position)
        {
            var guid = (ulong)0;
            var xyz = new XYZ(position.X, position.Y, position.Z);
            Functions.ClickToMove(GetPointerForGuid(GetLocalPlayerGuid()), (uint)ClickType.Move, ref guid, ref xyz, 2);
        }

        public void DoString(string luaCode) =>
            _mainThread.Invoke(() =>
                Functions.DoString(luaCode, Offsets.Functions.DoString));
    }
}
