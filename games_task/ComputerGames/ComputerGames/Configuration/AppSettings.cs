using Newtonsoft.Json;

namespace ComputerGames.Configuration
{
    public class AppSettings
    {
        [JsonProperty("Address")]
        public string? Address { get; set; }

        [JsonProperty("Port")]
        public uint Port { get; set; }

        [JsonProperty("StaticFilesPath")]
        public string? StaticFilesPath { get; set; }
    }
}
