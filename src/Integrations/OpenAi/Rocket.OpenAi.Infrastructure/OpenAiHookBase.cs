using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Integrations.Common;

namespace Rocket.OpenAi.Infrastructure
{
    public abstract class OpenAiHookBase<TExecutionStep, TConnector>
        : HookWithConnectorBase<TExecutionStep, TConnector>
        where TExecutionStep : BaseExecutionStep
        where TConnector : BaseConnector
    {
        protected readonly IOpenAiClient OpenAiClient;

        protected OpenAiHookBase(
            IOpenAiClient openAiClient,
            ILogger logger
        )
            : base(logger)
        {
            OpenAiClient = openAiClient;
        }

        protected async Task EnsureModelsExistAsync(
            string endpoint,
            CancellationToken cancellationToken,
            params string[] models
        )
        {
            // check if OpenAI has the model
            var modelList =
                (await
                    OpenAiClient
                        .GetModelListAsync(
                            endpoint,
                            cancellationToken
                        ))
                .Select(o => o.Id)
                .ToList();

            foreach (var model in models)
            {
                if (!modelList.Contains(model))
                    throw new RocketException(
                        $"The OpenAI connector configured does not support the model [{model}]. ",
                        ApiStatusCodeEnum.ThirdPartyServiceError
                    );
            }
        }
    }
}