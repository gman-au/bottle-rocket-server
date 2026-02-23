using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

namespace Rocket.Replicate.Domain
{
    public record ReplicateConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.OcrExtraction;

        public override string ConnectorName { get; set; } = ReplicateDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = ReplicateDomainConstants.ConnectorCode;

        public string ApiToken { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return !string.IsNullOrEmpty(ApiToken)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}