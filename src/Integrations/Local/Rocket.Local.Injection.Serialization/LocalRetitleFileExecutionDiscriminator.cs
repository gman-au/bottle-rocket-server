using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Local.Contracts;

namespace Rocket.Local.Injection.Serialization
{
    public class LocalRetitleFileExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(LocalRetitleFileExecutionStepSpecifics);
        
        public string Value => "local_retitle_file_execution";
    }
}