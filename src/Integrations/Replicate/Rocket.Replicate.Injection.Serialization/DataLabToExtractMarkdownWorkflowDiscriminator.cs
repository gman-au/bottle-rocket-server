using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.DataLabTo.Markdown;

namespace Rocket.Replicate.Injection.Serialization
{
    public class DataLabToExtractMarkdownWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(DataLabToExtractMarkdownWorkflowStepSpecifics);
        
        public string Value => "replicate_data_lab_to_extract_markdown_workflow";
    }
}