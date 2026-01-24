using System.Collections.Generic;
using Rocket.Domain.Core.Enum;

namespace Rocket.Domain.Core.Utils
{
    public static class DomainConstants
    {
        public const string RootAdminUserName = "admin";
        public const string Basic = "Basic";
        public const string BasicAuthentication = "BasicAuthentication";
        public const string BlazorAppCorsPolicy = "BlazorApp";

        public const string AdminRole = "Administrator";
        public const string UserRole = "User";

        public const string AuthHeaderKey = "authHeader";
        public const string UsernameKey = "userName";
        public const string RoleKey = "userRole";

        public const string ConnectorNameDropboxApi = "Dropbox App Connector";
        public const string ConnectorCodeDropboxApi = "DROPBOX_APP";
        
        public const string UnknownType = "Unknown";

        public static readonly Dictionary<int, string> ConnectorTypes = new()
        {
            { (int)ConnectorTypeEnum.FileForwarding, "File Forwarding" },
            { (int)ConnectorTypeEnum.FileConversion, "File Conversion" },
            { (int)ConnectorTypeEnum.OcrExtraction, "OCR Extraction" },
        };

        public static readonly Dictionary<int, string> WorkflowFormatTypes = new()
        {
            { (int)WorkflowFormatTypeEnum.Void, "No Data" },
            { (int)WorkflowFormatTypeEnum.File, "File Data" },
            { (int)WorkflowFormatTypeEnum.ImageData, "Image Data" },
            { (int)WorkflowFormatTypeEnum.RawTextData, "Raw Text Data" },
        };

        public static readonly Dictionary<int, string> ExecutionStatusTypes = new()
        {
            { (int)ExecutionStatusEnum.NotRun, "Not run" },
            { (int)ExecutionStatusEnum.Running, "Running" },
            { (int)ExecutionStatusEnum.Completed, "Completed" },
            { (int)ExecutionStatusEnum.Errored, "Errored" },
            { (int)ExecutionStatusEnum.Cancelled, "Cancelled" }
        };

        public static readonly Dictionary<int, string> BookVendorTypes = new()
        {
            { (int)BookVendorTypeEnum.Rocketbook, "Rocketbook" }
        };

    }
}