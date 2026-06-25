using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class ScansResponse
    {
        [JsonPropertyName("total_scans_received")]
        public int? TotalScansReceived { get; set; }
        
        [JsonPropertyName("scans_received_by_vendor")]
        public IEnumerable<ScanByVendorTotalSpecifics> ScansReceivedByVendor { get; set; }
    }
}