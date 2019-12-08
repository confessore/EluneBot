using KrycessBot.Enums;
using KrycessBot.Extensions;
using KrycessBot.Game.Interfaces;
using KrycessBot.Game.Models;
using KrycessBot.Models;
using KrycessBot.Services.Interfaces;
using KrycessBot.Statics;
using Process.NET;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace KrycessBot.Services
{
    public sealed class MemoryService : IMemoryService
    {
        readonly ProcessSharp processSharp;
        readonly IObjectManager objectManager;

        public MemoryService(
            ProcessSharp processSharp,
            IObjectManager objectManager)
        {
            this.processSharp = processSharp;
            this.objectManager = objectManager;
            enumerateVisibleObjectsCallbackDelegate = EnumerateVisibleObjectsCallback;
            enumerateVisibleObjectsCallbackPointer = Marshal.GetFunctionPointerForDelegate(enumerateVisibleObjectsCallbackDelegate);
        }

        delegate int EnumerateVisibleObjectsCallbackDelegate(int filter, ulong guid);
        readonly EnumerateVisibleObjectsCallbackDelegate enumerateVisibleObjectsCallbackDelegate;
        readonly IntPtr enumerateVisibleObjectsCallbackPointer;

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
        public Task<IntPtr> GetPointerforGuidAsync(ulong guid) =>
            Task.FromResult(Functions.GetPointerForGuid(guid));

        /// <summary>
        /// gets the object type from the pointer to the object
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns>Task<WoWObjectType></returns>
        public Task<WoWObjectType> GetWoWObjectType(IntPtr pointer) =>
            Task.FromResult((WoWObjectType)processSharp.Memory.Read<byte>(IntPtr.Add(pointer, (int)Offsets.ObjectManager.ObjType)));

        /// <summary>
        /// enumerates the visible objects if the guid system (object manager)
        /// </summary>
        /// <returns></returns>
        public async Task EnumerateVisibleObjectsAsync()
        {
            if (await IsInGameAsync())
            {
                var guid = await GetLocalPlayerGuidAsync();
                if (guid != 0)
                {
                    var playerPointer = await GetPointerforGuidAsync(guid);
                    if (playerPointer != IntPtr.Zero)
                    {
                        if (objectManager.LocalPlayer == null || objectManager.LocalPlayer.Pointer != playerPointer)
                            objectManager.LocalPlayer = new LocalPlayer(guid, playerPointer, WoWObjectType.OT_PLAYER);
                    }
                }
                foreach (var @object in objectManager.Objects.Values)
                    @object.CanRemove = true;
                Functions.EnumerateVisibleObjects(enumerateVisibleObjectsCallbackPointer, 0);
                foreach (var kvp in objectManager.Objects.Where(p => p.Value.CanRemove).ToList())
                    objectManager.Objects.Remove(kvp.Key);
                objectManager.FinalObjects = objectManager.Objects.Values.ToList();
            }
        }

        /// <summary>
        /// the callback for the enumerate visible objects function
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        int EnumerateVisibleObjectsCallback(int filter, ulong guid)
        {
            if (guid == 0) return 0;
            var pointer = GetPointerforGuidAsync(guid).GetAwaiter().GetResult();
            var type = GetWoWObjectType(pointer).GetAwaiter().GetResult();
            if (objectManager.Objects.ContainsKey(guid))
            {
                objectManager.Objects[guid].Pointer = pointer;
                objectManager.Objects[guid].CanRemove = false;
            }
            switch (type)
            {
                case WoWObjectType.OT_CONTAINER:
                    break;
                case WoWObjectType.OT_GAMEOBJ:
                    objectManager.Objects.Add(guid, new WoWGameObject(guid, pointer, type));
                    break;
                case WoWObjectType.OT_ITEM:
                    break;
                case WoWObjectType.OT_PLAYER:
                    break;
                case WoWObjectType.OT_UNIT:
                    break;
                default:
                    break;
            }
            return 1;
        }
    }
}
