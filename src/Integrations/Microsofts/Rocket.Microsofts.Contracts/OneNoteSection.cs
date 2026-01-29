using System.Text.Json.Serialization;

namespace Rocket.Microsofts.Contracts
{
    public class OneNoteSection
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("section_name")]
        public string SectionName { get; set; }
    }
}