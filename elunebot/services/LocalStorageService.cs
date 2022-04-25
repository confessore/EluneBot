using elunebot.models;
using elunebot.services.interfaces;
using elunebot.statics;
using System.IO;
using System.Text.Json;

namespace elunebot.services
{
    sealed class LocalStorageService : ILocalStorageService
    {
        public Settings ReadSettings()
        {
            try
            {
                return JsonSerializer.Deserialize<Settings>(File.ReadAllText(Paths.Settings));
            }
            catch { return null; }
        }

        public void WriteSettings(Settings settings) =>
            File.WriteAllTextAsync(Paths.Settings, JsonSerializer.Serialize(settings));
    }
}
