using KrycessBot.Enums;
using KrycessBot.Extensions;
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
        public readonly ProcessSharp processSharp;

        public MemoryService(ProcessSharp processSharp)
        {
            this.processSharp = processSharp;
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
        /// <returns>Task<long></long></returns>
        public Task<long> GetLocalPlayerGuid() =>
            Task.FromResult((long)Functions.GetLocalPlayerGuid());

        /// <summary>
        /// selects a character at the character selection screen
        /// </summary>
        /// <returns>Task</returns>
        public Task SelectCharacter() =>
            Task.FromResult(Functions.SelectCharacter());

        /// <summary>
        /// casts a spell at a location with a given rank
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="location"></param>
        /// <param name="rank"></param>
        /// <returns>Task</returns>
        public Task CastAtPosition(string spell, Location location, int rank = -1)
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
        public Task UseAtPosition(string spell, Location location,int rank = -1)
        {
            Functions.CastOrUseAtPosition(location.ToStruct());
            return Task.CompletedTask;
        }

        /// <summary>
        /// gets the base of the guid system (entity | object manager)
        /// </summary>
        /// <returns>Task<IntPtr></IntPtr></returns>
        internal async Task<IntPtr> GetPointerforGuid()
        {
            if (await IsInGameAsync())
                return await Task.FromResult(Functions.GetPointerForGuid());
            return await Task.FromResult(IntPtr.Zero);
        }
    }
}
