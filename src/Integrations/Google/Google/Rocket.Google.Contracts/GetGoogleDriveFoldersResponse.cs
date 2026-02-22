using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Google.Contracts
{
    public class GetGoogleDriveFoldersResponse : ApiResponse
    {
        [JsonPropertyName("folders")]
        public IEnumerable<GoogleDriveFolder> Folders { get; set; }
    }
}