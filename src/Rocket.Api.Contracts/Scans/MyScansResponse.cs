using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Scans
{
    public class MyScansResponse : ApiResponse
    {
        [JsonPropertyName("scans")]
        public IEnumerable<MyScanItem> Scans { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}