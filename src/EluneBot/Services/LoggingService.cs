using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EluneBot.Services
{
    public class LoggingService : ILoggingService
    {
        public async Task GeneralLog(string input, bool showDate = true) =>
            await Log(Paths.GeneralLog, input, showDate);
        public async Task GeneralLog(int input, bool showDate = true) =>
            await Log(Paths.GeneralLog, input, showDate);

        public Task Log(string path, string input, bool showDate = true)
        {
            var tmp = (showDate ? "[" + DateTime.Now + "] " : "") + input + Environment.NewLine;
            File.AppendAllText(path, tmp);
            return Task.CompletedTask;
        }
        public Task Log(string path, int input, bool showDate = true)
        {
            var tmp = (showDate ? "[" + DateTime.Now + "] " : "") + input + Environment.NewLine;
            File.AppendAllText(path, tmp);
            return Task.CompletedTask;
        }
    }
}
