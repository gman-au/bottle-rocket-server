using System.Text.Json.Serialization;

namespace Rocket.Microsofts.Contracts
{
    public class GetOneNoteSectionsRequest
    {
        [JsonPropertyName("connector_id")]
        public string ConnectorId { get; set; }
    }
}