using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.QuestPdf.Contracts;

namespace Rocket.QuestPdf.Injection.Serialization
{
    public class ConvertToPdfExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(ConvertToPdfExecutionStepSpecifics);
        
        public string Value => "convert_to_pdf_execution";
    }
}