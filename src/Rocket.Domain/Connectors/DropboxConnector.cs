using Rocket.Domain.Enum;

namespace Rocket.Domain.Connectors
{
    public class DropboxConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;
        
        public override string ConnectorName { get; set; } = "Dropbox";
        
        public string AccessToken { get; set; }
    }
}