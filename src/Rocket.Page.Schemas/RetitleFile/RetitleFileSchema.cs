using System.Text.Json.Serialization;

namespace Rocket.Page.Schemas.RetitleFile
{
    public class RetitleFileSchema
    {
        [JsonPropertyName("recommended_title")]
        public string RecommendedTitle { get; set; }
    }
}