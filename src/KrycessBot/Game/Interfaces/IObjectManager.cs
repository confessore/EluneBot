using KrycessBot.Game.Models;
using System.Collections.Generic;

namespace KrycessBot.Game.Interfaces
{
    public interface IObjectManager
    {
        Dictionary<ulong, WoWObject> Objects { get; set; }
        List<WoWObject> FinalObjects { get; set; }
        LocalPlayer LocalPlayer { get; set; }
    }
}
