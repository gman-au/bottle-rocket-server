using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Mailgun.Contracts;

namespace Rocket.Mailgun.Injection.Serialization
{
    public class MailgunSendEmailWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(MailgunSendEmailWorkflowStepSpecifics);
        
        public string Value => "mailgun_send_email_workflow";
    }
}