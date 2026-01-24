using Rocket.Domain.Core;
using Rocket.Domain.Core.Enum;

namespace Rocket.MaxOcr.Domain
{
    public record MaxOcrConnector : CoreConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.OcrExtraction;

        public override string ConnectorName { get; set; } = MaxOcrDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = MaxOcrDomainConstants.ConnectorCode;

        public string Endpoint { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return !string.IsNullOrEmpty(Endpoint)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}