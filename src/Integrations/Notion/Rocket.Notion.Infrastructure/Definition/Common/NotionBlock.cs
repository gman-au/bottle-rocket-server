using System.Text.Json.Serialization;
using Rocket.Notion.Infrastructure.Definition.FileUpload;

namespace Rocket.Notion.Infrastructure.Definition.Common
{
    [JsonDerivedType(typeof(NotionParagraphBlock))]
    [JsonDerivedType(typeof(NotionImageFileUploadBlock))]
    public abstract class NotionBlock
    {
    }
}