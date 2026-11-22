using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenAI;
using Rocket.Interfaces;
using Rocket.OpenAi.Infrastructure.Definition;

namespace Rocket.OpenAi.Infrastructure
{
    public class OpenAiClient(
        ISchemaResponseBuilder schemaResponseBuilder,
        ISchemaGenerator schemaGenerator,
        IImageBase64Converter imageBase64Converter,
        ILogger<OpenAiClient> logger
    ) : IOpenAiClient
    {
        private const string SystemRole = "system";
        private const string UserRole = "user";

        private static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<IEnumerable<OpenAiTagsModel>> GetModelListAsync(
            string apiKey,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var openAi = new OpenAIClient(apiKey);

                var modelClient =
                    openAi
                        .GetOpenAIModelClient();

                var models =
                    await
                        modelClient
                            .GetModelsAsync(cancellationToken);

                var results =
                    models
                        .Value
                        .Select(o => new OpenAiTagsModel { Id = o.Id });

                return results;
            }
            catch (Exception ex)
            {
                logger
                    .LogError("There was an error retrieving the OpenAI models: {message}", ex.Message);
                
                throw;
            }
        }
    }
}