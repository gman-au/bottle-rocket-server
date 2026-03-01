using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;

namespace Rocket.Microsofts.Injection.Serialization
{
    public class OneNoteUploadWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(OneNoteUploadWorkflowStepSpecifics);
        
        public string Value => "one_note_upload_workflow";
    }
}