using System.Text.Json.Serialization;

namespace Rocket.Notion.Contracts
{
    public class NotionParentNoteSummary
    {
        [JsonPropertyName("parent_note_id")]
        public string ParentNoteId { get; set; }
        
        [JsonPropertyName("parent_note_name")]
        public string ParentNoteName { get; set; }
    }
}