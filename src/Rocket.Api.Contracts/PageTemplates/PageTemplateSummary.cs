using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.PageTemplates
{
    public class PageTemplateSummary
    {
        [JsonPropertyName("qr_code")]
        public string QrCode { get; set; }

        [JsonPropertyName("book_vendor")]
        public string BookVendor { get; set; }
    }
}