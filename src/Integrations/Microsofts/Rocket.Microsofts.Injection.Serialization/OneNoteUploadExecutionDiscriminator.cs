using System;
using Rocket.Domain.Executions;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;

namespace Rocket.Microsofts.Injection.Serialization
{
    public class OneNoteUploadExecutionDiscriminator : IJsonTypeDiscriminator<BaseExecutionStep>
    {
        public Type Key => typeof(OneNoteUploadExecutionStepSpecifics);
        
        public string Value => "one_note_upload_execution";
    }
}