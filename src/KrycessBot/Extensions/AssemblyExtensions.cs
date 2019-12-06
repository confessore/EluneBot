using System.IO;
using System.Reflection;

namespace KrycessBot.Extensions
{
    internal static class AssemblyExtensions
    {
        public static string JumpUp(this Assembly value, int levels)
        {
            var tmp = value.Location;
            for (var i = 0; i < levels; i++)
                tmp = Path.GetDirectoryName(tmp);
            return tmp;
        }
    }
}
