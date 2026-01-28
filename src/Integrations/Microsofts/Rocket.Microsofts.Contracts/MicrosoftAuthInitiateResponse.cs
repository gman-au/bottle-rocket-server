using System.Text.Json.Serialization;
using Rocket.Api.Contracts;

namespace Rocket.Microsofts.Contracts
{
    public class MicrosoftAuthInitiateResponse : ApiResponse
    {
        [JsonPropertyName("result")]
        public MicrosoftDeviceCodeResult Result { get; set; }
    }
}