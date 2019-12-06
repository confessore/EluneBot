using KrycessBot.Enums;
using KrycessBot.Services.Interfaces;
using KrycessBot.Statics;
using Process.NET;
using System.Threading.Tasks;

namespace KrycessBot.Services
{
    internal sealed class MemoryService : IMemoryService
    {
        public readonly ProcessSharp processSharp;

        public MemoryService(ProcessSharp processSharp)
        {
            this.processSharp = processSharp;
        }

        public Task<bool> IsInGameAsync() =>
            Task.FromResult(processSharp.Memory.Read<bool>(Offsets.LocalPlayer.IsInGame));

        public Task<WoWClass> ClassAsync() =>
            Task.FromResult((WoWClass)processSharp.Memory.Read<byte>(Offsets.LocalPlayer.Class));

        public Task<ulong> GetLocalPlayerGuid() =>
            Task.FromResult((ulong)Functions.GetLocalPlayerGuid());
    }
}
