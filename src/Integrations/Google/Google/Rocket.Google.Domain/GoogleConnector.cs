using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Extensions;

namespace Rocket.Google.Domain
{
    public record GoogleConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = GoogleDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = GoogleDomainConstants.ConnectorCode;

        public GooglesCredential Credential { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            if (new[] {Credential?.Installed?.ClientId, Credential?.Installed?.ClientSecret, AccessToken, RefreshToken}.IsErrorProperty())
                return ConnectorStatusEnum.Error;
            
            return !string.IsNullOrEmpty(RefreshToken)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}