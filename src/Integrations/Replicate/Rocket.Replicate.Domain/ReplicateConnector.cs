using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Extensions;

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
            if (ApiToken.IsErrorProperty())
                return ConnectorStatusEnum.Error;
            
            return !string.IsNullOrEmpty(ApiToken)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}