using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.DataLabTo.Markdown;

namespace Rocket.Replicate.Injection.Serialization
{
    public class DataLabToExtractMarkdownExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(DataLabToExtractMarkdownExecutionStepSpecifics);
        
        public string Value => "replicate_data_lab_to_extract_markdown_execution";
    }
}