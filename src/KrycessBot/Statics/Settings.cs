using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace KrycessBot.Statics
{
    internal static class Settings
    {
        public static string WoWPath
        {
            get =>
                JObject.Parse(File.ReadAllText(Paths.Settings)).SelectToken(typeof(Settings).Name).Value<string>(MethodBase.GetCurrentMethod().Name.Replace("get_", string.Empty));
            set
            {
                JObject settingsJObject = JObject.Parse(File.ReadAllText(Paths.Settings));
                JToken defaultJToken = settingsJObject.SelectToken(typeof(Settings).Name);
                defaultJToken[MethodBase.GetCurrentMethod().Name.Replace("set_", string.Empty)] = value;
                File.WriteAllText(Paths.Settings, JsonConvert.SerializeObject(settingsJObject, Formatting.Indented));
            }
        }
    }
}
