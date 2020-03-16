using EluneBot.Enums;
using EluneBot.Models;
using Process.NET;
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
        Task ClickToMoveAsync(Location position);
        Task EnumerateVisibleObjectsAsync(IntPtr callback, int filter);
        Task DoStringAsync(string luaCode);
    }
}
