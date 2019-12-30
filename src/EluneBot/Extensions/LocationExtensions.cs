using EluneBot.Models;
using EluneBot.Structs;

namespace EluneBot.Extensions
{
    public static class LocationExtensions
    {
        public static XYZ ToStruct(this Location position) =>
            new XYZ(position.X, position.Y, position.Z);
    }
}
