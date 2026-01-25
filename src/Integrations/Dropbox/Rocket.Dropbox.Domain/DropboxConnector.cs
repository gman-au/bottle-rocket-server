using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;

namespace Rocket.Dropbox.Domain
{
    public record DropboxConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = DropboxDomainConstants.ConnectorName;
        
        public override string ConnectorCode { get; set; } = DropboxDomainConstants.ConnectorCode;

        public string AppKey { get; set; }

        public string AppSecret { get; set; }

        public string RefreshToken { get; set; }

        public override ConnectorStatusEnum DetermineStatus()
        {
            return !string.IsNullOrEmpty(RefreshToken)
                ? ConnectorStatusEnum.Active
                : ConnectorStatusEnum.Pending;
        }
    }
}