using KrycessBot.Enums;
using KrycessBot.Models;
using System.Threading.Tasks;

namespace KrycessBot.Services.Interfaces
{
    public interface IMemoryService
    {
        Task<bool> IsInGameAsync();
        Task<WoWClass> ClassAsync();
        Task<long> GetLocalPlayerGuid();
        Task SelectCharacter();
        Task CastAtPosition(string spell, Location location, int rank = -1);
        Task UseAtPosition(string spell, Location location, int rank = -1);
    }
}
