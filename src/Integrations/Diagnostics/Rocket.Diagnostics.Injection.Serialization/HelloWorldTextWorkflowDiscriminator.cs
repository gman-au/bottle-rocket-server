using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Serialization
{
    public class HelloWorldTextWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(HelloWorldTextWorkflowStepSpecifics);

        public string Value => "hello_world_text_workflow";
    }
}