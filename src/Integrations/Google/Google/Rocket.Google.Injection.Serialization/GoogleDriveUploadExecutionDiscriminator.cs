using System;
using Rocket.Domain.Executions;
using Rocket.Google.Contracts;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Serialization
{
    public class GoogleDriveUploadExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(GoogleDriveUploadExecutionStepSpecifics);
        
        public string Value => "google_drive_upload_execution";
    }
}