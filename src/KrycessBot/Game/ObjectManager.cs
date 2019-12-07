using KrycessBot.Game.Interfaces;
using KrycessBot.Game.Models;
using System.Collections.Generic;

namespace KrycessBot.Game
{
    public sealed class ObjectManager : IObjectManager
    {
        public LocalPlayer LocalPlayer { get; set; }
        readonly Dictionary<ulong, WoWObject> WoWObjectsobjects = new Dictionary<ulong, WoWObject>();
    }
}
