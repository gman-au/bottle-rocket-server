using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.DataLabTo;

namespace Rocket.Replicate.Injection.Serialization
{
    public class DataLabToExtractTextExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(DataLabToExtractTextExecutionStepSpecifics);
        
        public string Value => "replicate_data_lab_to_extract_text_execution";
    }
}