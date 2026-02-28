using System;
using Rocket.Domain.Workflows;
using Rocket.Dropbox.Contracts;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection.Serialization
{
    public class DropboxUploadWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(DropboxUploadWorkflowStepSpecifics);
        
        public string Value => "dropbox_upload_workflow";
    }
}