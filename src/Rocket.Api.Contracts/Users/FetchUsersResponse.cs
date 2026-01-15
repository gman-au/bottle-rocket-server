using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Users
{
    public class FetchUsersResponse : ApiResponse
    {
        [JsonPropertyName("users")]
        public IEnumerable<UserItem> Users { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}