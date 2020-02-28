using EluneBot.Structs;
using System;

namespace EluneBot.Models
{
    public class Location
    {
        public Location(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Location(XYZ xyz)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public float DistanceTo(Location location)
        {
            var deltaX = X - location.X;
            var deltaY = Y - location.Y;
            var deltaZ = Z - location.Z;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }
    }
}
