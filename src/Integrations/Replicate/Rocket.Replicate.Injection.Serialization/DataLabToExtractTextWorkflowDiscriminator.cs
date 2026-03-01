using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts.Models.DataLabTo;

namespace Rocket.Replicate.Injection.Serialization
{
    public class DataLabToExtractTextWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(DataLabToExtractTextWorkflowStepSpecifics);
        
        public string Value => "replicate_data_lab_to_extract_text_workflow";
    }
}