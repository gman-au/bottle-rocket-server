using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

namespace Rocket.Microsofts.Domain
{
    public record MicrosoftConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = MicrosoftDomainConstants.ConnectorName;

        public override string ConnectorCode { get; set; } = MicrosoftDomainConstants.ConnectorCode;

        public string ClientId { get; set; }
        
        public string TenantId { get; set; }

        public string RefreshToken { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return !string.IsNullOrEmpty(RefreshToken)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}