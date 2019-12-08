using KrycessBot.Enums;
using KrycessBot.Models;
using System;
using System.Threading.Tasks;

namespace KrycessBot.Services.Interfaces
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
        Task EnumerateVisibleObjectsAsync();
    }
}
