using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class ScanByVendorTotalSpecifics
    {
        [JsonPropertyName("vendor")]
        public string Vendor { get; set; }
        
        [JsonPropertyName("scans")]
        public int Scans { get; set; }
    }
}