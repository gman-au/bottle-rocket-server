using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Postmark.Contracts;

namespace Rocket.Postmark.Injection.Serialization
{
    public class PostmarkSendEmailExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(PostmarkSendEmailExecutionStepSpecifics);
        
        public string Value => "postmark_send_email_execution";
    }
}