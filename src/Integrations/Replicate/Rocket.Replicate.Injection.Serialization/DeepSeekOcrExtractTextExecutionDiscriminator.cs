using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.DeepSeekOcr;

namespace Rocket.Replicate.Injection.Serialization
{
    public class DeepSeekOcrExtractTextExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(DeepSeekOcrExtractTextExecutionStepSpecifics);
        
        public string Value => "replicate_deep_seek_ocr_extract_text_execution";
    }
}