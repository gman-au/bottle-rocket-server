using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Local.Contracts;

namespace Rocket.Local.Injection.Serialization
{
    public class LocalUploadWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(LocalUploadWorkflowStepSpecifics);
        
        public string Value => "local_upload_workflow";
    }
}