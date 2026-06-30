using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

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
            return !string.IsNullOrEmpty(ApiKey) && !string.IsNullOrEmpty(SenderAddress) && !string.IsNullOrEmpty(SenderDomain)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}