using EluneBot.Models;
using System.Collections.Generic;

namespace EluneBot.Services.Interfaces
{
    public interface IObjectManagerService
    {
        Dictionary<ulong, WoWObject> Objects { get; set; }
        List<WoWObject> FinalObjects { get; set; }
        LocalPlayer LocalPlayer { get; set; }
    }
}
