using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.QuestPdf.Contracts;

namespace Rocket.QuestPdf.Injection.Serialization
{
    public class ConvertToPdfWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(ConvertToPdfWorkflowStepSpecifics);
        
        public string Value => "convert_to_pdf_workflow";
    }
}