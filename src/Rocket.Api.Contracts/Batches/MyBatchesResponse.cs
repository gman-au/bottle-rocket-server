using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Batches
{
    public class MyBatchesResponse : ApiResponse
    {
        [JsonPropertyName("batches")]
        public IEnumerable<BatchSummary> Batches { get; set; }
        
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }
    }
}