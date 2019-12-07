using KrycessBot.Enums;
using KrycessBot.Extensions;
using KrycessBot.Game.Interfaces;
using KrycessBot.Game.Models;
using KrycessBot.Models;
using KrycessBot.Services.Interfaces;
using KrycessBot.Statics;
using Process.NET;
using System;
using System.Threading.Tasks;

namespace KrycessBot.Services
{
    internal sealed class MemoryService : IMemoryService
    {
        readonly ProcessSharp processSharp;
        //readonly IObjectManager objectManager;

        public MemoryService(
            ProcessSharp processSharp)
        //IObjectManager objectManager)
        {
            this.processSharp = processSharp;
            //this.objectManager = objectManager;
        }

        /// <summary>
        /// checks to see if the player is logged into the game world
        /// </summary>
        /// <returns>bool</returns>
        public Task<bool> IsInGameAsync() =>
            Task.FromResult(processSharp.Memory.Read<bool>(Offsets.LocalPlayer.IsInGame));

        /// <summary>
        /// gets the player's character class
        /// </summary>
        /// <returns>WoWClass</returns>
        public Task<WoWClass> ClassAsync() =>
            Task.FromResult((WoWClass)processSharp.Memory.Read<byte>(Offsets.LocalPlayer.Class));

        /// <summary>
        /// gets the player's guid
        /// </summary>
        /// <returns>Task<long></returns>
        public Task<long> GetLocalPlayerGuidAsync() =>
            Task.FromResult((long)Functions.GetLocalPlayerGuid());

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
        public async Task<IntPtr> GetPointerforGuidAsync(long guid)
        {
            if (await IsInGameAsync())
                return await Task.FromResult(Functions.GetPointerForGuid(guid));
            return await Task.FromResult(IntPtr.Zero);
        }

        /// <summary>
        /// enumerates the visible objects if the guid system (object manager)
        /// </summary>
        /// <returns></returns>
        public async Task EnumerateVisibleObjects()
        {
            if (!await IsInGameAsync()) return;
            var guid = await GetLocalPlayerGuidAsync();
            if (guid == 0) return;
            var playerPointer = await GetPointerforGuidAsync(guid);
            if (playerPointer == IntPtr.Zero) return;
            //if (objectManager.LocalPlayer == null || playerPointer != objectManager.LocalPlayer.Pointer)
            //    objectManager.LocalPlayer = new LocalPlayer(guid, playerPointer, WoWObjectType.OT_PLAYER);
        }
    }
}
