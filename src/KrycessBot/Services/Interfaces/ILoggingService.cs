using System.Threading.Tasks;

namespace KrycessBot.Services.Interfaces
{
    public interface ILoggingService
    {
        Task Log(string path, string input, bool showDate = true);
    }
}
