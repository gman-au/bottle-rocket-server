using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Markdig.Contracts;

namespace Rocket.Markdig.Injection.Serialization
{
    public class ConvertToEpubExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(ConvertToEpubExecutionStepSpecifics);
        
        public string Value => "convert_to_epub_execution";
    }
}