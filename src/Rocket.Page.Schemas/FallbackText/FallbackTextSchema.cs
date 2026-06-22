using System.Text.Json.Serialization;

namespace Rocket.Page.Schemas.FallbackText
{
    // Some OCR models *will not* follow instructions regarding dropping the codes and icons on the bottom of the page.
    // Use this schema to specifically instruct the OCR model to divide the page into "useful markdown" + "everything else".
    public class FallbackTextSchema
    {
        [JsonPropertyName("main_page")]
        public string MainPage { get; set; }
        
        [JsonPropertyName("bottom_navigation_bar")]
        public string BottomNavigationBar { get; set; }
        
        [JsonPropertyName("qr_code")]
        public string QrCode { get; set; }
    }
}