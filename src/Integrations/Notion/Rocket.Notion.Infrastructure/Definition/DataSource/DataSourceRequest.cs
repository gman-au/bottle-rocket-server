using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Notion.Infrastructure.Definition.Common;

namespace Rocket.Notion.Infrastructure.Definition.DataSource
{
    public class DataSourceRequest
    {
        [JsonPropertyName("parent")]
        public DataSourceParent Parent { get; set; }
        
        [JsonPropertyName("properties")]
        public Dictionary<string, NotionRichTextProperty> Properties { get; set; }
    }
}