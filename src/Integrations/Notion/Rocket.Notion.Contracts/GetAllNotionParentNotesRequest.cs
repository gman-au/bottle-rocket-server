using System.Text.Json.Serialization;

namespace Rocket.Notion.Contracts
{
    public class GetAllNotionParentNotesRequest
    {
        [JsonPropertyName("connector_id")]
        public string ConnectorId { get; set; }
    }
}