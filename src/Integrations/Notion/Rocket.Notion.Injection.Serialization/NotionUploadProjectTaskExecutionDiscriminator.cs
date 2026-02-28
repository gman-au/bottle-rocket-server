using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Notion.Contracts;

namespace Rocket.Notion.Injection.Serialization
{
    public class NotionUploadProjectTaskExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(NotionUploadProjectTaskExecutionStepSpecifics);
        
        public string Value => "notion_upload_project_task_execution";
    }
}