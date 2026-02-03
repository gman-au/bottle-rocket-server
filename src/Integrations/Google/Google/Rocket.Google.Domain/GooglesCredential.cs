using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Google.Domain
{
    public class GooglesCredential
    {
        [BsonElement("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [BsonElement("project_id")]
        [JsonPropertyName("project_id")]
        public string ProjectId { get; set; }
        
        [BsonElement("private_key_id")]
        [JsonPropertyName("private_key_id")]
        public string PrivateKeyId { get; set; }
        
        [BsonElement("private_key")]
        [JsonPropertyName("private_key")]
        public string PrivateKey { get; set; }
        
        [BsonElement("client_email")]
        [JsonPropertyName("client_email")]
        public string ClientEmail { get; set; }
        
        [BsonElement("client_id")]
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        
        [BsonElement("auth_uri")]
        [JsonPropertyName("auth_uri")]
        public string AuthUri { get; set; }
        
        [BsonElement("token_uri")]
        [JsonPropertyName("token_uri")]
        public string TokenUri { get; set; }
        
        [BsonElement("auth_provider_x509_cert_url")]
        [JsonPropertyName("auth_provider_x509_cert_url")]
        public string AuthProviderX509CertUrl { get; set; }
        
        [BsonElement("client_x509_cert_url")]
        [JsonPropertyName("client_x509_cert_url")]
        public string ClientX509CertUrl { get; set; }
        
        [BsonElement("universe_domain")]
        [JsonPropertyName("universe_domain")]
        public string UniverseDomain { get; set; }
    }
}