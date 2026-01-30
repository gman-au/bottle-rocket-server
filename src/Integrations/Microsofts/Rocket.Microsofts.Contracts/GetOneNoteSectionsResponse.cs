using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Microsofts.Contracts
{
    public class GetOneNoteSectionsResponse : ApiResponse
    {
        [JsonPropertyName("sections")]
        public IEnumerable<OneNoteSection> Sections { get; set; }
    }
}