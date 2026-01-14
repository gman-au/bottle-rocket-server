using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Dropbox.Contracts
{
    public class CreateDropboxConnectorResponse : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("authorize_uri")]
        public string AuthorizeUri { get; set; }
    }
}