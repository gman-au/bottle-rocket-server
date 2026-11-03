using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Extensions;

namespace Rocket.Mailgun.Domain
{
    public record MailgunConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = MailgunDomainConstants.ConnectorName;

        public override string ConnectorCode { get; set; } = MailgunDomainConstants.ConnectorCode;

        public string ApiKey { get; set; }

        public string SenderDomain { get; set; }

        public string SenderAddress { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            if (ApiKey.IsErrorProperty())
                return ConnectorStatusEnum.Error;

            return !string.IsNullOrEmpty(ApiKey) && !string.IsNullOrEmpty(SenderAddress) && !string.IsNullOrEmpty(SenderDomain)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}