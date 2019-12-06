using KrycessBot.Enums;
using System.Threading.Tasks;

namespace KrycessBot.Services.Interfaces
{
    public interface IMemoryService
    {
        Task<bool> IsInGameAsync();
        Task<WoWClass> ClassAsync();
    }
}
