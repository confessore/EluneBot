using KrycessBot.Models;
using KrycessBot.Structs;

namespace KrycessBot.Extensions
{
    public static class LocationExtensions
    {
        public static XYZ ToStruct(this Location position) =>
            new XYZ(position.X, position.Y, position.Z);
    }
}
