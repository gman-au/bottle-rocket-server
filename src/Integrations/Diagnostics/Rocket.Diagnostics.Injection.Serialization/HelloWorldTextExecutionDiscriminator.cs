using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Domain.Executions;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Serialization
{
    public class HelloWorldTextExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(HelloWorldTextExecutionStepSpecifics);

        public string Value => "hello_world_text_execution";
    }
}