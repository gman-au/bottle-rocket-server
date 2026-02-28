using System;
using Rocket.Domain.Workflows;
using Rocket.Google.Contracts;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Serialization
{
    public class GoogleDriveUploadWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(GoogleDriveUploadWorkflowStepSpecifics);
        
        public string Value => "google_drive_upload_workflow";
    }
}