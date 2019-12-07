using KrycessBot.Game.Interfaces;
using KrycessBot.Game.Models;
using KrycessBot.Services.Interfaces;
using System.Collections.Generic;

namespace KrycessBot.Game
{
    public sealed class ObjectManager : IObjectManager
    {
        readonly IMemoryService memoryService;

        public ObjectManager(IMemoryService memoryService)
        {
            this.memoryService = memoryService;
        }
        public LocalPlayer LocalPlayer { get; set; }
        readonly Dictionary<long, WoWObject> WoWObjectsobjects = new Dictionary<long, WoWObject>();
    }
}
