using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Users
{
    public class SetUserDarkModeRequest
    {
        [JsonPropertyName("dark_mode")]
        public bool DarkMode { get; set; }
    }
}