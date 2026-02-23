using Rocket.Interfaces;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Injection.Web
{
    public class OllamaConnectorProduct : ISkuConnector
    {
        public string Name => OllamaDomainConstants.ConnectorName;
        public string Href => "/MyConnector/Ollama/Add";
        public string ImagePath => "/img/ollama-logo.png";
    }
}