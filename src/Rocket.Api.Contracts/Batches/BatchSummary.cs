using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Scans;

namespace Rocket.Api.Contracts.Batches
{
    public class BatchSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("date_scanned")]
        public DateTime DateScanned { get; set; }
        
        [JsonPropertyName("scans")]
        public IEnumerable<ScanSummary> Scans { get; set; }
    }
}