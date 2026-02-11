using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Google.Domain
{
    public class GoogleInstalledCredential
    {
        [BsonElement("client_id")]
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        
        [BsonElement("project_id")]
        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }
        
        [BsonElement("auth_uri")]
        [JsonPropertyName("auth_uri")]
        public string AuthUri { get; set; }
        
        [BsonElement("token_uri")]
        [JsonPropertyName("token_uri")]        
        public string TokenUri { get; set; }
        
        [BsonElement("auth_provider_x509_cert_url")]
        [JsonPropertyName("auth_provider_x509_cert_url")]
        public string AuthProviderX509CertUrl { get; set; }

        [BsonElement("client_secret")]
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [BsonElement("redirect_uris")]
        [JsonPropertyName("redirect_uris")]
        public IEnumerable<string> RedirectUris { get; set; }
    }
}