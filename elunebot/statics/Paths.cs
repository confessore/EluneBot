using elunebot.extensions;
using System.Reflection;

namespace elunebot.statics
{
    static class Paths
    {
        public static Assembly Assembly = Assembly.GetExecutingAssembly();

        public static string Binary = Assembly.Location;

        public static string Settings = $"{Assembly.JumpUp(1)}\\{Strings.Settings}";

        public static string Nethost = $"{Assembly.JumpUp(1)}\\{Strings.Nethost}";
        public static string Bootstrap = $"{Assembly.JumpUp(1)}\\{Strings.Bootstrap}";
        public static string Fasm = $"{Assembly.JumpUp(1)}\\{Strings.Fasm}";
        public static string FastCall = $"{Assembly.JumpUp(1)}\\{Strings.FastCall}";
        public static string Navigation = $"{Assembly.JumpUp(1)}\\{Strings.Navigation}";

        public static string BotBases = $"{Assembly.JumpUp(1)}\\{Strings.BotBases}";
        public static string Logs = $"{Assembly.JumpUp(1)}\\{Strings.Logs}";
        public static string Plugins = $"{Assembly.JumpUp(1)}\\{Strings.Plugins}";

        //public static string DefaultLog = $"{Log}\\default.log";
    }
}
