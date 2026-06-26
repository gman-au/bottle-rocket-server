using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rocket.Api.Contracts.Dashboard
{
    public class LifecycleResponse
    {
        [JsonPropertyName("lifecycles_by_group")]
        public IEnumerable<LifecycleTotalSpecifics> LifecyclesByGroup { get; set; }
    }
}