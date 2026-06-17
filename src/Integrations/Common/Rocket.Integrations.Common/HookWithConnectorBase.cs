using Microsoft.Extensions.Logging;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;
using Rocket.Interfaces;

namespace Rocket.Integrations.Common
{
    public abstract class HookWithConnectorBase<TExecutionStep, TConnector>
        : HookBase<TExecutionStep>
        where TExecutionStep : BaseExecutionStep
        where TConnector : BaseConnector
    {
        protected TConnector Connector;

        protected HookWithConnectorBase(
            ILogger logger,
            IFileRetitler fileRetitler = null
        ) : base(
            logger,
            fileRetitler
        )
        {
        }

        public void SetConnector(TConnector connector)
        {
            Connector =
                connector ??
                throw new RocketException(
                    $"Unexpected connector provided to hook [{typeof(TExecutionStep)}: {typeof(TConnector)}], please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            Logger
                .LogDebug(
                    "Connector of type {connector} set for hook type {type}",
                    typeof(TConnector).Name,
                    typeof(TExecutionStep).Name
                );
        }
    }
}