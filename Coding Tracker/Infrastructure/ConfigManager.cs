using Microsoft.Extensions.Configuration;

namespace Coding_Tracker.Infrastructure
{
    internal class ConfigManager
    {
        public ConfigurationManager Config { get; } = new();
        public ConfigManager()
        {
            Config.Sources.Clear();
            Config.AddJsonFile("appsettings.json", optional: false);
        }
    }
}