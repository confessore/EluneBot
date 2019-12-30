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
        Task<IntPtr> GetPointerforGuidAsync(ulong guid);
        Task<WoWObjectType> GetWoWObjectTypeAsync(IntPtr pointer);
        void EnumerateVisibleObjects(IntPtr callback, int filter);
    }
}
