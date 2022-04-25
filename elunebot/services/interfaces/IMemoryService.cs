using elunebot.models;
using elunebot.models.enums;
using System;

namespace elunebot.services.interfaces
{
    public interface IMemoryService
    {
        bool IsInGame();

        /// <summary>
        /// gets the player's guid
        /// </summary>
        /// <returns>ulong</returns>
        ulong GetLocalPlayerGuid();

        /// <summary>
        /// gets the base of the guid system (object manager)
        /// </summary>
        /// <returns>IntPtr</returns>
        IntPtr GetPointerForGuid(ulong guid);

        /// <summary>
        /// gets the object type from the pointer to the object
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns>WoWObjectType</returns>
        WoWObjectType GetWoWObjectType(IntPtr pointer);

        /// <summary>
        /// enumerates all visible objects around the player
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="filter"></param>
        void EnumerateVisibleObjects(IntPtr callback, int filter);

        /// <summary>
        /// clicks to move the player
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        void ClickToMove(Location position);

        void DoString(string luaCode);
    }
}
