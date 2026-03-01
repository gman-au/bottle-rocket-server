using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Integrations.Common;

namespace Rocket.Ollama.Infrastructure
{
    public abstract class OllamaHookBase<TExecutionStep, TConnector>
        : HookWithConnectorBase<TExecutionStep, TConnector>
        where TExecutionStep : BaseExecutionStep
        where TConnector : BaseConnector
    {
        protected readonly IOllamaClient OllamaClient;

        protected OllamaHookBase(
            IOllamaClient ollamaClient,
            ILogger logger
        )
            : base(logger)
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