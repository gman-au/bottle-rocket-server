using Rocket.Domain.Enum;
using Rocket.Domain.Utils;

namespace Rocket.Domain.Connectors
{
    public class DropboxConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;
        
        public override string ConnectorName { get; set; } = DomainConstants.VendorDropbox;
        
        public string AccessToken { get; set; }
    }
}