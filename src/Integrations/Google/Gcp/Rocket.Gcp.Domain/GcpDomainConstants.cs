using Rocket.Domain.Enum;

namespace Rocket.Gcp.Domain
{
    public static class GcpDomainConstants
    {
        public const string ConnectorName = "Google Cloud Platform Connector";
        public const string ConnectorCode = "GCP_PLATFORM";
        
        public const string ExtractTextWorkflowCode = "GCP_EXTRACT_TEXT";
        
        public static readonly int[] GcpExtractInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int GcpExtractOutputType = (int)WorkflowFormatTypeEnum.RawTextData;
    }
}