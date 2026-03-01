using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Domain.Executions;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Serialization
{
    public class HelloWorldProjectExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(HelloWorldProjectExecutionStepSpecifics);

        public string Value => "hello_world_project_execution";
    }
}