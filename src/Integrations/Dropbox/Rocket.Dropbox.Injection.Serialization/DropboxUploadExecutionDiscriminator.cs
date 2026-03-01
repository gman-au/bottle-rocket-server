using System;
using Rocket.Domain.Executions;
using Rocket.Dropbox.Contracts;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection.Serialization
{
    public class DropboxUploadExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(DropboxUploadExecutionStepSpecifics);
        
        public string Value => "dropbox_upload_execution";
    }
}