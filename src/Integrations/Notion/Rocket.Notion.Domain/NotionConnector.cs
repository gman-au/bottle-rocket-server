using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Extensions;

namespace Rocket.Notion.Domain
{
    public record NotionConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = NotionDomainConstants.ConnectorName;

        public override string ConnectorCode { get; set; } = NotionDomainConstants.ConnectorCode;

        public string IntegrationSecret { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            if (IntegrationSecret.IsErrorProperty())
                return ConnectorStatusEnum.Error;
            
            return !string.IsNullOrEmpty(IntegrationSecret)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}