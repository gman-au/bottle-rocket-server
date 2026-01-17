using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Rocket.Api.Contracts.Workflows;
using Rocket.Dropbox.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class RocketTypeInfoResolver
    {
        public static readonly DefaultJsonTypeInfoResolver Instance =
            new()
            {
                Modifiers =
                {
                    static typeInfo =>
                    {
                        if (typeInfo.Type == typeof(WorkflowStepSummary))
                        {
                            typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                            {
                                TypeDiscriminatorPropertyName = "$type",
                                DerivedTypes =
                                {
                                    Build<DropboxUploadStepSpecifics>("dropbox_upload"),
                                    Build<EmailFileAttachmentStepSpecifics>("email_file_attachment")
                                    // ... register all derived types here
                                }
                            };
                        }
                    }
                }
            };
 
        public static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            TypeInfoResolver = Instance
        };
        
        private static JsonDerivedType Build<T>(string discriminator) => new(typeof(T), discriminator);
    }
}