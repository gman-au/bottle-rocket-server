using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Notion.Contracts
{
    public class GetAllNotionParentNotesResponse : ApiResponse
    {
        [JsonPropertyName("parent_notes")]
        public IEnumerable<NotionParentNoteSummary> ParentNotes { get; set; }
    }
}