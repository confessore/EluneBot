using System.Reflection;

namespace elunebot.statics
{
    static class Strings
    {
        public const string Process = "wow";

        public static string ExecutingName = Assembly.GetExecutingAssembly().GetName().Name;
        public static string ExecutingVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string EntryLocation = Assembly.GetExecutingAssembly().Location;

        public const string Settings = "settings.json";

        public const string Nethost = "nethost.dll";
        public const string Bootstrap = "elunebot.bootstrap.dll";
        public const string Fasm = "fasmdll_managed.dll";
        public const string FastCall = "elunebot.fastcall.dll";
        public const string Navigation = "elunebot.navigation.dll";

        public const string BotBases = "botbases";
        public const string Logs = "logs";
        public const string Plugins = "plugins";
    }
}
