using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Google.Domain
{
    public class GooglesCredential
    {
        [BsonElement("installed")]
        [JsonPropertyName("installed")]
        public GoogleInstalledCredential Installed { get; set; }
    }
}