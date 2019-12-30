using System.Threading.Tasks;

namespace EluneBot.Services.Interfaces
{
    public interface ILoggerService
    {
        Task GeneralLog(string input, bool showDate = true);
        Task GeneralLog(int input, bool showDate = true);
        Task Log(string path, string input, bool showDate = true);
        Task Log(string path, int input, bool showDate = true);
    }
}
