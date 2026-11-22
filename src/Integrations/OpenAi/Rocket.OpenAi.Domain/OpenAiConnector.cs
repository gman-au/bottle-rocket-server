using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

namespace Rocket.OpenAi.Domain
{
    public record OpenAiConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.OcrExtraction;

        public override string ConnectorName { get; set; } = OpenAiDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = OpenAiDomainConstants.ConnectorCode;

        public string ApiKey { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return !string.IsNullOrEmpty(ApiKey)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}