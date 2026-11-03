using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Extensions;

namespace Rocket.Gcp.Domain
{
    public record GcpConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.OcrExtraction;

        public override string ConnectorName { get; set; } = GcpDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = GcpDomainConstants.ConnectorCode;

        public GcpCredential Credential { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            if (new[] {Credential?.PrivateKey, Credential?.PrivateKeyId, Credential?.ClientId}.IsErrorProperty())
                return ConnectorStatusEnum.Error;
            
            return Credential != null
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}