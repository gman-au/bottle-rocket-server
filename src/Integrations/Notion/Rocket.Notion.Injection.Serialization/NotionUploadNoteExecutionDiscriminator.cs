using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Notion.Contracts;

namespace Rocket.Notion.Injection.Serialization
{
    public class NotionUploadNoteExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(NotionUploadNoteExecutionStepSpecifics);
        
        public string Value => "notion_upload_note_execution";
    }
}