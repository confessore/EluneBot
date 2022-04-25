using elunebot.extensions;
using elunebot.models;
using elunebot.models.enums;
using elunebot.services.interfaces;
using elunebot.statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace elunebot.services
{
    sealed class ObjectManagerService : IObjectManagerService
    {
        readonly IMemoryService _memory;
        readonly IMainThreadService _mainThread;

        public ObjectManagerService(
            IMemoryService memory,
            IMainThreadService mainThread)
        {
            _memory = memory;
            _mainThread = mainThread;
            enumerateVisibleObjectsCallbackDelegate = EnumerateVisibleObjectsCallback;
            enumerateVisibleObjectsCallbackPointer = Marshal.GetFunctionPointerForDelegate(enumerateVisibleObjectsCallbackDelegate);
            _ = Task.Run(() =>
            {
                while (true)
                {
                    _mainThread.Invoke(() => EnumerateVisibleObjects());
                }
            });
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        delegate int EnumerateVisibleObjectsCallbackDelegate(int filter, ulong guid);
        readonly EnumerateVisibleObjectsCallbackDelegate enumerateVisibleObjectsCallbackDelegate;
        readonly IntPtr enumerateVisibleObjectsCallbackPointer;

        public IDictionary<ulong, WoWObject> Objects { get; set; } = new Dictionary<ulong, WoWObject>();
        public IEnumerable<WoWObject> FinalObjects { get; set; } = new List<WoWObject>();
        public LocalPlayer LocalPlayer { get; set; }
        public LocalPet LocalPet { get; set; }

        public IEnumerable<WoWUnit> Units =>
            FinalObjects.OfType<WoWUnit>();

        public IEnumerable<WoWUnit> Players =>
            FinalObjects.OfType<WoWUnit>().Where(x => x.Type == WoWObjectType.OT_PLAYER);

        public IEnumerable<WoWUnit> NPCs =>
            FinalObjects.OfType<WoWUnit>().Where(x => x.Type == WoWObjectType.OT_UNIT);

        public IEnumerable<WoWGameObject> GameObjects =>
            FinalObjects.OfType<WoWGameObject>();

        public IEnumerable<WoWItem> Items =>
            FinalObjects.OfType<WoWItem>();

        /// <summary>
        /// enumerates the visible objects if the guid system (object manager)
        /// </summary>
        /// <returns></returns>
        public void EnumerateVisibleObjects()
        {
            if (_memory.IsInGame())
            {
                var guid = _memory.GetLocalPlayerGuid();
                if (guid != 0)
                {
                    var playerPointer = _memory.GetPointerForGuid(guid);
                    if (playerPointer != IntPtr.Zero)
                    {
                        if (LocalPlayer == null || LocalPlayer.Pointer != playerPointer)
                            LocalPlayer = new LocalPlayer(guid, playerPointer, WoWObjectType.OT_PLAYER);
                    }
                }
                foreach (var @object in Objects.Values)
                    @object.CanRemove = true;
                _memory.EnumerateVisibleObjects(enumerateVisibleObjectsCallbackPointer, -1);
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
        public int EnumerateVisibleObjectsCallback(int filter, ulong guid)
        {
            if (guid == 0) return 0;
            var pointer = _memory.GetPointerForGuid(guid);
            if (pointer == IntPtr.Zero) return 0;
            var type = _memory.GetWoWObjectType(pointer);
            if (Objects.ContainsKey(guid))
            {
                Objects[guid].Pointer = pointer;
                Objects[guid].CanRemove = false;
            }
            else
            {
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
                        Objects.Add(guid, new WoWUnit(guid, pointer, type));
                        break;
                    case WoWObjectType.OT_UNIT:
                        Objects.Add(guid, new WoWUnit(guid, pointer, type));
                        var owner = pointer.Add(Offsets.ObjectManager.DescriptorOffset)
                            .PointsTo()
                            .Add(Offsets.Descriptors.SummonedByGuid)
                            .ReadAs<ulong>();
                        if (LocalPlayer != null && owner == LocalPlayer.Guid)
                        {
                            if (LocalPet != null && LocalPet.Pointer == pointer) break;
                            LocalPet = new LocalPet(guid, pointer, type);
                        }
                        break;
                    default:
                        break;
                }
            }
            return 1;
        }
    }
}
