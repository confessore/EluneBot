using EluneBot.Enums;
using EluneBot.Models;
using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EluneBot.Services
{
    public sealed class ObjectManagerService : IObjectManagerService
    {
        readonly ILoggerService logger;
        readonly IMemoryService memory;
        public ObjectManagerService(
            ILoggerService logger,
            IMemoryService memory)
        {
            this.logger = logger;
            this.memory = memory;
            enumerateVisibleObjectsCallbackDelegate = EnumerateVisibleObjectsCallback;
            enumerateVisibleObjectsCallbackPointer = Marshal.GetFunctionPointerForDelegate(enumerateVisibleObjectsCallbackDelegate);
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await EnumerateVisibleObjectsAsync();
                    await Task.Delay(50);
                }
            });
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate int EnumerateVisibleObjectsCallbackDelegate(ulong guid, int filter);
        readonly EnumerateVisibleObjectsCallbackDelegate enumerateVisibleObjectsCallbackDelegate;
        readonly IntPtr enumerateVisibleObjectsCallbackPointer;

        public Dictionary<ulong, WoWObject> Objects { get; set; } = new Dictionary<ulong, WoWObject>();
        public List<WoWObject> FinalObjects { get; set; } = new List<WoWObject>();
        public LocalPlayer LocalPlayer { get; set; }

        /// <summary>
        /// enumerates the visible objects if the guid system (object manager)
        /// </summary>
        /// <returns></returns>
        public async Task EnumerateVisibleObjectsAsync()
        {
            if (await memory.IsInGameAsync())
            {
                var guid = await memory.GetLocalPlayerGuidAsync();
                if (guid != 0)
                {
                    var playerPointer = await memory.GetPointerForGuidAsync(guid);
                    var tmp = await memory.GetPointerForGuidAsync(185586967084269567);
                    if (playerPointer != IntPtr.Zero)
                    {
                        if (LocalPlayer == null || LocalPlayer.Pointer != playerPointer)
                            LocalPlayer = new LocalPlayer(guid, playerPointer, WoWObjectType.OT_PLAYER);
                    }
                }
                foreach (var @object in Objects.Values)
                    @object.CanRemove = true;
                await memory.EnumerateVisibleObjects(enumerateVisibleObjectsCallbackPointer, -1);
                foreach (var kvp in Objects.Where(p => p.Value.CanRemove).ToList())
                    Objects.Remove(kvp.Key);
                FinalObjects = Objects.Values.ToList();
            }
        }

        /// <summary>
        /// the callback for the enumerate visible objects function
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public int EnumerateVisibleObjectsCallback(ulong guid, int filter)
        {
            if (guid == 0) return 0;
            var pointer = memory.GetPointerForGuidAsync(guid).GetAwaiter().GetResult();
            if (pointer == IntPtr.Zero) return 0;
            var type = memory.GetWoWObjectTypeAsync(pointer).GetAwaiter().GetResult();
            if (Objects.ContainsKey(guid))
            {
                Objects[guid].Pointer = pointer;
                Objects[guid].CanRemove = false;
            }
            switch (type)
            {
                case WoWObjectType.OT_CONTAINER:
                    break;
                case WoWObjectType.OT_CORPSE:
                    break;
                case WoWObjectType.OT_GAMEOBJ:
                    Objects.Add(guid, new WoWGameObject(guid, pointer, type));
                    break;
                case WoWObjectType.OT_ITEM:
                    Objects.Add(guid, new WoWItem(guid, pointer, type));
                    break;
                case WoWObjectType.OT_NONE:
                    break;
                case WoWObjectType.OT_PLAYER:
                    break;
                case WoWObjectType.OT_UNIT:
                    Objects.Add(guid, new WoWUnit(guid, pointer, type));
                    break;
                default:
                    break;
            }
            logger.GeneralLog($"{guid} {pointer} {type}");
            return 1;
        }
    }
}
