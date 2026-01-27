using System.Text.Json.Serialization;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    [JsonDerivedType(typeof(NotionParagraphBlock))]
    public abstract class NotionBlock
    {
    }
}