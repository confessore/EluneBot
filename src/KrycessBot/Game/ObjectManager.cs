using KrycessBot.Game.Interfaces;
using KrycessBot.Game.Models;
using System.Collections.Generic;

namespace KrycessBot.Game
{
    public sealed class ObjectManager : IObjectManager
    {
        public Dictionary<ulong, WoWObject> Objects { get; set; } = new Dictionary<ulong, WoWObject>();
        public List<WoWObject> FinalObjects { get; set; } = new List<WoWObject>();
        public LocalPlayer LocalPlayer { get; set; }
        
    }
}
