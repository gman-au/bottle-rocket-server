using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Serialization
{
    public class HelloWorldProjectWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(HelloWorldProjectWorkflowStepSpecifics);

        public string Value => "hello_world_project_workflow";
    }
}