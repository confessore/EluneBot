using elunebot.models;

namespace elunebot.services.interfaces
{
    interface ILocalStorageService
    {
        Settings ReadSettings();

        void WriteSettings(Settings settings);
    }
}
