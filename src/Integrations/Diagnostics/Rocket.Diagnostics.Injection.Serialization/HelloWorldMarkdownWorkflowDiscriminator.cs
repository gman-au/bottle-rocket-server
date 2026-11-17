using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Serialization
{
    public class HelloWorldMarkdownWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(HelloWorldMarkdownWorkflowStepSpecifics);

        public string Value => "hello_world_markdown_workflow";
    }
}