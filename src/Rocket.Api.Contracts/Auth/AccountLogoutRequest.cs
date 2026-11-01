using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Auth
{
    public class AccountLogoutRequest
    {
        [JsonPropertyName("logout_all")]
        public bool? LogoutAll { get; set; }
    }
}