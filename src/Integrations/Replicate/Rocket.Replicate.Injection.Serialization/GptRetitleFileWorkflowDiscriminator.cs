using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.Gpt;

namespace Rocket.Replicate.Injection.Serialization
{
    public class GptRetitleFileWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(GptRetitleFileWorkflowStepSpecifics);
        
        public string Value => "replicate_gpt_retitle_file_workflow";
    }
}