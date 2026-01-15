using System.Text.Json.Serialization.Metadata;
using Rocket.Api.Contracts.Workflows;
using Rocket.Dropbox.Contracts;

namespace Rocket.Api.Host.Json
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
                                    Build<EmailFileAttachmentStep>("email_file_attachment")
                                    // ... register all derived types here
                                }
                            };
                        }
                    }
                }
            };
        
        private static JsonDerivedType Build<T>(string discriminator) => new(typeof(T), discriminator);
    }
}