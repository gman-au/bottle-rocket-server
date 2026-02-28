using System;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;
using Rocket.Notion.Contracts;

namespace Rocket.Notion.Injection.Serialization
{
    public class NotionUploadProjectTaskWorkflowDiscriminator : IJsonTypeDiscriminator<BaseWorkflowStep>
    {
        public Type Key => typeof(NotionUploadProjectTaskWorkflowStepSpecifics);
        
        public string Value => "notion_upload_project_task_workflow";
    }
}