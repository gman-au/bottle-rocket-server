using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.DataLabTo;

namespace Rocket.Replicate.Injection.Serialization
{
    public class DataLabToExtractProjectExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(DataLabToExtractProjectExecutionStepSpecifics);
        
        public string Value => "replicate_data_lab_to_extract_project_execution";
    }
}