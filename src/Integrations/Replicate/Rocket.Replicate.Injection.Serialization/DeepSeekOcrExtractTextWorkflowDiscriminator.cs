using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.DeepSeekOcr;

namespace Rocket.Replicate.Injection.Serialization
{
    public class DeepSeekOcrExtractTextWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(DeepSeekOcrExtractTextWorkflowStepSpecifics);
        
        public string Value => "replicate_deep_seek_ocr_extract_text_workflow";
    }
}