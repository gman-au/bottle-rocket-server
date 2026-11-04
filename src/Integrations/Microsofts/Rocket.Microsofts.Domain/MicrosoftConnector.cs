using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Extensions;

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
            if (RefreshToken.IsErrorProperty())
                return ConnectorStatusEnum.Error;
            
            return !string.IsNullOrEmpty(RefreshToken)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}