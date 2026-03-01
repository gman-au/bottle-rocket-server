using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;

namespace Rocket.Microsofts.Injection.Serialization
{
    public class OneDriveUploadWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OneDriveUploadWorkflowStepSpecifics);
        
        public string Value => "one_drive_upload_workflow";
    }
}