using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Mailgun.Contracts;

namespace Rocket.Mailgun.Injection.Serialization
{
    public class MailgunSendEmailExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(MailgunSendEmailExecutionStepSpecifics);
        
        public string Value => "mailgun_send_email_execution";
    }
}