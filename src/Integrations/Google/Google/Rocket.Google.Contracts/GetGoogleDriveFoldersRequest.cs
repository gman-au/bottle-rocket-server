using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Google.Contracts
{
    public class GetGoogleDriveFoldersRequest : ApiResponse
    {
        [JsonPropertyName("connector_id")]
        public string ConnectorId { get; set; }
    }
}