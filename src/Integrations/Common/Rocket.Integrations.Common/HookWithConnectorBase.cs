using Microsoft.Extensions.Logging;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Executions;

namespace Rocket.Integrations.Common
{
    public abstract class HookWithConnectorBase<TExecutionStep, TConnector>
        : HookBase<TExecutionStep>
        where TExecutionStep : BaseExecutionStep
        where TConnector : BaseConnector
    {
        protected HookWithConnectorBase(ILogger logger) : base(logger) { }

        protected TConnector Connector;

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