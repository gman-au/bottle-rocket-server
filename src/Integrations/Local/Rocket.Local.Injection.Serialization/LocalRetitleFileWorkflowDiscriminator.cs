using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Local.Contracts;

namespace Rocket.Local.Injection.Serialization
{
    public class LocalRetitleFileWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(LocalRetitleFileWorkflowStepSpecifics);
        
        public string Value => "local_retitle_file_workflow";
    }
}