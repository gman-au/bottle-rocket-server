using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Local.Contracts;

namespace Rocket.Local.Injection.Serialization
{
    public class LocalUploadExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(LocalUploadExecutionStepSpecifics);
        
        public string Value => "local_upload_execution";
    }
}