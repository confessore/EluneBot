using EluneBot.Models;

namespace EluneBot.Services.Interfaces
{
    public interface INavigationService
    {
        Location[] CalculateLocation(uint mapId, Location start, Location end, bool straightPath);
    }
}
