using System;
using Rocket.Domain.Workflows;
using Rocket.Gcp.Contracts;
using Rocket.Interfaces;

namespace Rocket.Gcp.Injection.Serialization
{
    public class GcpExtractWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(GcpExtractWorkflowStepSpecifics);
        
        public string Value => "gcp_extract_workflow";
    }
}