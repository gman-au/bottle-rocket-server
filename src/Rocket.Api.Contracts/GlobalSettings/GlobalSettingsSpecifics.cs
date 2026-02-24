using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.GlobalSettings
{
    public class GlobalSettingsSpecifics : ApiResponse
    {
        [JsonPropertyName("sweep_successful_scans_after_days")]
        public int? SweepSuccessfulScansAfterDays { get; set; }

        [JsonPropertyName("enable_sweeping")]
        public bool? EnableSweeping { get; set; }
    }
}