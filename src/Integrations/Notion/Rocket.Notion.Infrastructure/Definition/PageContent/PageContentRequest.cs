using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Notion.Infrastructure.Definition.Common;

namespace Rocket.Notion.Infrastructure.Definition.PageContent
{
    public class PageContentRequest
    {
        [JsonPropertyName("parent")]
        public PageContentParent Parent { get; set; }
        
        [JsonPropertyName("properties")]
        public NotionProperties Properties { get; set; }
        
        [JsonPropertyName("children")]
        public IEnumerable<NotionBlock> Children { get; set; }
    }
}