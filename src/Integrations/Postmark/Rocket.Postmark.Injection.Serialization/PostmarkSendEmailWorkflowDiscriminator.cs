using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Postmark.Contracts;

namespace Rocket.Postmark.Injection.Serialization
{
    public class PostmarkSendEmailWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(PostmarkSendEmailWorkflowStepSpecifics);
        
        public string Value => "postmark_send_email_workflow";
    }
}