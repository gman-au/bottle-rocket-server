using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

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
            return !string.IsNullOrEmpty(IntegrationSecret)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}