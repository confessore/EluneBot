using EluneBot.Extensions;
using System.Reflection;

namespace EluneBot.Statics
{
    public static class Paths
    {
        public static Assembly Assembly = Assembly.GetExecutingAssembly();

        public static string Binary = Assembly.Location;

        public static string Settings = $"{Assembly.JumpUp(1)}\\{Strings.Settings}";

        public static string FastCall = $"{Assembly.JumpUp(1)}\\{Strings.FastCall}";
        public static string Injector = $"{Assembly.JumpUp(1)}\\{Strings.Injector}";
        public static string Loader = $"{Assembly.JumpUp(1)}\\{Strings.Loader}";
        public static string Navigation = $"{Assembly.JumpUp(1)}\\{Strings.Navigation}";

        public static string Bases = $"{Assembly.JumpUp(1)}\\{Strings.Bases}";
        public static string Logs = $"{Assembly.JumpUp(1)}\\{Strings.Logs}";
        public static string Plugins = $"{Assembly.JumpUp(1)}\\{Strings.Plugins}";

        public static string GeneralLog = $"{Logs}\\{Strings.GeneralLog}";
    }
}
