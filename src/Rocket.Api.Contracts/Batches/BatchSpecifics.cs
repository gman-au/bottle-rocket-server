using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Scans;

namespace Rocket.Api.Contracts.Batches
{
    public class BatchSpecifics : ApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("date_scanned")]
        public DateTime DateScanned { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }
        
        [JsonPropertyName("scans")]
        public IEnumerable<ScanSummary> Scans { get; set; }
        
        [JsonPropertyName("scan_count")]
        public int ScanCount { get; set; }
    }
}