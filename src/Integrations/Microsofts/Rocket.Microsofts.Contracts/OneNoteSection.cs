using System.Text.Json.Serialization;

namespace Rocket.Microsofts.Contracts
{
    public class OneNoteSection
    {
        [JsonPropertyName("section_id")]
        public string SectionId { get; set; }
        
        [JsonPropertyName("section_name")]
        public string SectionName { get; set; }
    }
}