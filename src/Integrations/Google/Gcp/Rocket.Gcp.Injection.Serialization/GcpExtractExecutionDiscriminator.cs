using System;
using Rocket.Domain.Executions;
using Rocket.Gcp.Contracts;
using Rocket.Interfaces;

namespace Rocket.Gcp.Injection.Serialization
{
    public class GcpExtractExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(GcpExtractExecutionStepSpecifics);
        
        public string Value => "gcp_extract_execution";
    }
}