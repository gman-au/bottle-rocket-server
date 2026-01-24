using System;
using System.Collections.Generic;
using Rocket.Dropbox.Contracts;
using Rocket.MaxOcr.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class ExecutionStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadExecutionStepSpecifics), "dropbox_upload_execution" },
            { typeof(MaxOcrExtractExecutionStepSpecifics), "maxocr_extract_execution" }
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