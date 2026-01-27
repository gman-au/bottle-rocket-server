using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.PageContent
{
    public class PageContentParent
    {
        [JsonPropertyName("page_id")] 
        public string PageId { get; set; }
    }
}