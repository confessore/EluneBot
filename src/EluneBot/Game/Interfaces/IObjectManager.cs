using EluneBot.Game.Models;
using System.Collections.Generic;

namespace EluneBot.Game.Interfaces
{
    public interface IObjectManager
    {
        Dictionary<ulong, WoWObject> Objects { get; set; }
        List<WoWObject> FinalObjects { get; set; }
        LocalPlayer LocalPlayer { get; set; }
    }
}
