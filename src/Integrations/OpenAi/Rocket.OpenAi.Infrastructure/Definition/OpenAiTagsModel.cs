using System.Text.Json.Serialization;

namespace Rocket.OpenAi.Infrastructure.Definition
{
    public class OpenAiTagsModel
    {
        [JsonPropertyName("Id")] 
        public string Id { get; set; }
    }
}