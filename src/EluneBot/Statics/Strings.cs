using System.Reflection;

namespace EluneBot.Statics
{
    internal static class Strings
    {
        public const string Process = "WoW";

        public static string ExecutingName = Assembly.GetExecutingAssembly().GetName().Name;
        public static string ExecutingVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string EntryLocation = Assembly.GetExecutingAssembly().Location;

        public const string Settings = "Settings.json";

        public const string FastCall = "EluneBot.FastCall.dll";
        public const string Injector = "EluneBot.Injector.dll";
        public const string Loader = "EluneBot.Loader.dll";
        public const string Navigation = "EluneBot.Navigation.dll";

        public const string Bases = "Bases";
        public const string Logs = "Logs";
        public const string Plugins = "Plugins";

        public const string GeneralLog = "GeneralLog.txt";
    }
}
