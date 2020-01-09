using EluneBot.Enums;
using EluneBot.Models;
using System;
using System.Threading.Tasks;

namespace EluneBot.Services.Interfaces
{
    public interface IMemoryService
    {
        Task<bool> IsInGameAsync();
        Task<WoWClass> ClassAsync();
        Task<ulong> GetLocalPlayerGuidAsync();
        Task SelectCharacterAsync();
        Task CastAtPositionAsync(string spell, Location location, int rank = -1);
        Task UseAtPositionAsync(string spell, Location location, int rank = -1);
        Task<IntPtr> GetPointerForGuidAsync(ulong guid);
        Task<WoWObjectType> GetWoWObjectTypeAsync(IntPtr pointer);
        Task EnumerateVisibleObjects(IntPtr callback, int filter);
    }
}
