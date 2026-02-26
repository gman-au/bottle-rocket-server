using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaHookBase
    {
        protected readonly IOllamaClient OllamaClient;

        protected OllamaHookBase(IOllamaClient ollamaClient)
        {
            OllamaClient = ollamaClient;
        }

        protected async Task EnsureModelsExistAsync(
            string endpoint,
            CancellationToken cancellationToken,
            params string[] models
        )
        {
            // check if Ollama has the model
            var modelList =
                (await
                    OllamaClient
                        .GetModelListAsync(
                            endpoint,
                            cancellationToken
                        ))
                .ToList();

            foreach (var model in models)
            {
                if (!modelList.Contains(model))
                    throw new RocketException(
                        $"The Ollama instance does not have model [{model}] pulled. " +
                        $"Ensure that the model is downloaded via the command 'ollama pull {model}'.",
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );
            }
        }
    }
}