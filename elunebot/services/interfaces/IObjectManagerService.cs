using elunebot.models;
using System.Collections.Generic;

namespace elunebot.services.interfaces
{
    public interface IObjectManagerService
    {
        IDictionary<ulong, WoWObject> Objects { get; set; }
        IEnumerable<WoWObject> FinalObjects { get; set; }
        LocalPlayer LocalPlayer { get; set; }
        LocalPet LocalPet { get; set; }

        IEnumerable<WoWUnit> Units { get; }

        IEnumerable<WoWUnit> Players { get; }

        IEnumerable<WoWUnit> NPCs { get; }

        IEnumerable<WoWGameObject> GameObjects { get; }

        IEnumerable<WoWItem> Items { get; }

        /// <summary>
        /// enumerates the visible objects if the guid system (object manager)
        /// </summary>
        /// <returns></returns>
        void EnumerateVisibleObjects();

        /// <summary>
        /// the callback for the enumerate visible objects function
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        int EnumerateVisibleObjectsCallback(int filter, ulong guid);
    }
}
