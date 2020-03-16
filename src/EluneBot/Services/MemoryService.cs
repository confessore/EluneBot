using EluneBot.Enums;
using EluneBot.Extensions;
using EluneBot.Models;
using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using System;
using System.Threading.Tasks;

namespace EluneBot.Services
{
    public sealed class MemoryService : IMemoryService
    {
        readonly IMainThreadService mainThread;

        public MemoryService(IMainThreadService mainThread)
        {
            this.mainThread = mainThread;
        }

        /// <summary>
        /// checks to see if the player is logged into the game world
        /// </summary>
        /// <returns>bool</returns>
        public Task<bool> IsInGameAsync() =>
            Task.FromResult(App.ProcessSharp.Memory.Read<bool>(Offsets.LocalPlayer.IsInGame));

        /// <summary>
        /// gets the player's character class
        /// </summary>
        /// <returns>WoWClass</returns>
        public Task<WoWClass> ClassAsync() =>
            Task.FromResult((WoWClass)App.ProcessSharp.Memory.Read<byte>(Offsets.LocalPlayer.Class));

        /// <summary>
        /// gets the player's guid
        /// </summary>
        /// <returns>Task<long></returns>
        public Task<ulong> GetLocalPlayerGuidAsync() =>
                Task.FromResult((ulong)Functions.GetLocalPlayerGuid());

        /// <summary>
        /// selects a character at the character selection screen
        /// </summary>
        /// <returns>Task</returns>
        public Task SelectCharacterAsync() =>
            Task.FromResult(Functions.SelectCharacter());

        /// <summary>
        /// casts a spell at a location with a given rank
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="location"></param>
        /// <param name="rank"></param>
        /// <returns>Task</returns>
        public Task CastAtPositionAsync(string spell, Location location, int rank = -1)
        {
            Functions.CastOrUseAtPosition(location.ToStruct());
            return Task.CompletedTask;
        }

        /// <summary>
        /// uses an item at a location
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="location"></param>
        /// <param name="rank"></param>
        /// <returns>Task</returns>
        public Task UseAtPositionAsync(string spell, Location location, int rank = -1)
        {
            Functions.CastOrUseAtPosition(location.ToStruct());
            return Task.CompletedTask;
        }

        /// <summary>
        /// gets the base of the guid system (object manager)
        /// </summary>
        /// <returns>Task<IntPtr></returns>
        public Task<IntPtr> GetPointerForGuidAsync(ulong guid) =>
            Task.FromResult(Functions.GetPointerForGuid(guid));

        /// <summary>
        /// gets the object type from the pointer to the object
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns>Task<WoWObjectType></returns>
        public Task<WoWObjectType> GetWoWObjectTypeAsync(IntPtr pointer) =>
            Task.FromResult((WoWObjectType)App.ProcessSharp.Memory.Read<byte>(IntPtr.Add(pointer, (int)Offsets.ObjectManager.ObjType)));

        /// <summary>
        /// clicks to move the player
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public async Task ClickToMoveAsync(Location position)
        {
            var guid = (ulong)0;
            var xyz = position.ToStruct();
            Functions.ClickToMove(await GetPointerForGuidAsync(await GetLocalPlayerGuidAsync()), (uint)ClickType.Move, ref guid, ref xyz, 2);
        }

        /// <summary>
        /// enumerates all visible objects around the player
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="filter"></param>
        public Task EnumerateVisibleObjectsAsync(IntPtr callback, int filter)
        {
            Functions.EnumerateVisibleObjects(callback, filter, Offsets.Functions.EnumerateVisibleObjects);
            return Task.CompletedTask;
        }

        public Task DoStringAsync(string luaCode)
        {
            mainThread.Invoke(() => Functions.DoString(luaCode, Offsets.Functions.DoString));
            return Task.CompletedTask;
        }
    }
}
