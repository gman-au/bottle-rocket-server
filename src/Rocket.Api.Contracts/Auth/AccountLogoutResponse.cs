using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Auth
{
    public class AccountLogoutResponse : ApiResponse
    {
        [JsonPropertyName("is_logged_out")]
        public bool IsLoggedOut { get; set; }
    }
}