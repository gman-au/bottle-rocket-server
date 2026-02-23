using System.Text.Json.Serialization;

namespace Rocket.Replicate.Infrastructure.Definition
{
    public class CreatePredictionRequest<T> where T : IReplicateInput
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }
        
        [JsonPropertyName("input")]
        public T Input { get; set; }
    }
}