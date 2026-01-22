using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.PageTemplates
{
    public class FetchPageTemplatesResponse : ApiResponse
    {
        [JsonPropertyName("templates")]
        public IEnumerable<PageTemplateSummary> Templates { get; set; }
    }
}