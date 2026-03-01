using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Notion.Contracts;

namespace Rocket.Notion.Injection.Serialization
{
    public class NotionUploadNoteWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(NotionUploadNoteWorkflowStepSpecifics);
        
        public string Value => "notion_upload_note_workflow";
    }
}