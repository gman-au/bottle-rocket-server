using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Markdig.Contracts;

namespace Rocket.Markdig.Injection.Serialization
{
    public class ConvertToEpubWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(ConvertToEpubWorkflowStepSpecifics);
        
        public string Value => "convert_to_epub_workflow";
    }
}