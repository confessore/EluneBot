using EluneBot.Models;
using System.Collections.Generic;

namespace EluneBot.Services.Interfaces
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
    }
}
