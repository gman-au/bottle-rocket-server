using System;
using System.Collections.Generic;
using Rocket.Dropbox.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class ExecutionStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadExecutionStepSpecifics), "dropbox_upload_execution" }
        };

        public static string GetExecutionStepTypeDiscriminator(this Type type)
        {
            return
                TypeDiscriminatorMap
                    .GetValueOrDefault(
                        type,
                        "base"
                    );
        }
    }
}