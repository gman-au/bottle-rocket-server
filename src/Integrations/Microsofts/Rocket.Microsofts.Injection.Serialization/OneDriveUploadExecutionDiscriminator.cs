using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;

namespace Rocket.Microsofts.Injection.Serialization
{
    public class OneDriveUploadExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OneDriveUploadExecutionStepSpecifics);
        
        public string Value => "one_drive_upload_execution";
    }
}