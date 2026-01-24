using Rocket.Domain.Core.Enum;
using Rocket.Domain.Core.Utils;

namespace Rocket.Domain.Connectors
{
    public record DropboxConnector : BaseConnector
    {
        public override int ConnectorType { get; set; } = (int)ConnectorTypeEnum.FileForwarding;

        public override string ConnectorName { get; set; } = DomainConstants.ConnectorNameDropboxApi;
        
        public override string ConnectorCode { get; set; } = DomainConstants.ConnectorCodeDropboxApi;

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