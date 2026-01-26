using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

namespace Rocket.Ollama.Domain
{
    public record OllamaConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.OcrExtraction;

        public override string ConnectorName { get; set; } = OllamaDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = OllamaDomainConstants.ConnectorCode;

        public string Endpoint { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return !string.IsNullOrEmpty(Endpoint)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}