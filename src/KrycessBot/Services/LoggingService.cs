using KrycessBot.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KrycessBot.Services
{
    public class LoggingService : ILoggingService
    {
        public Task Log(string path, string input, bool showDate = true)
        {
            var tmp = (showDate ? "[" + DateTime.Now + "] " : "") + input + Environment.NewLine;
            File.AppendAllText(path, tmp);
            return Task.CompletedTask;
        }
    }
}
