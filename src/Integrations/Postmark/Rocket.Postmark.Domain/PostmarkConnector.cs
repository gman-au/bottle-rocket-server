using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Extensions;

namespace Rocket.Postmark.Domain
{
    public record PostmarkConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = PostmarkDomainConstants.ConnectorName;

        public override string ConnectorCode { get; set; } = PostmarkDomainConstants.ConnectorCode;

        public string ServerToken { get; set; }
        
        public string SenderAddress { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            if (ServerToken.IsErrorProperty())
                return ConnectorStatusEnum.Error;
            
            return !string.IsNullOrEmpty(ServerToken) && !string.IsNullOrEmpty(SenderAddress)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}