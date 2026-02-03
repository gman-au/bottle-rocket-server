using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

namespace Rocket.Google.Domain
{
    public record GoogleConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = GoogleDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = GoogleDomainConstants.ConnectorCode;

        public GooglesCredential Credential { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return Credential != null
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}