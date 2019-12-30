using EluneBot.Game.Interfaces;
using EluneBot.Game.Models;
using System.Collections.Generic;

namespace EluneBot.Game
{
    public sealed class ObjectManager : IObjectManager
    {
        public Dictionary<ulong, WoWObject> Objects { get; set; } = new Dictionary<ulong, WoWObject>();
        public List<WoWObject> FinalObjects { get; set; } = new List<WoWObject>();
        public LocalPlayer LocalPlayer { get; set; }
        
    }
}
