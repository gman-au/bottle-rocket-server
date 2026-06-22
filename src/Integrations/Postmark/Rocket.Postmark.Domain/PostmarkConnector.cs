using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

namespace Rocket.Postmark.Domain
{
    public record PostmarkConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = PostmarkDomainConstants.ConnectorName;

        public override string ConnectorCode { get; set; } = PostmarkDomainConstants.ConnectorCode;

        public string IntegrationSecret { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return !string.IsNullOrEmpty(IntegrationSecret)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}