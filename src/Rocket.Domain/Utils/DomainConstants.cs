using System.Collections.Generic;
using Rocket.Domain.Enum;

namespace Rocket.Domain.Utils
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

        public const string VendorDropbox = "Dropbox";

        public static readonly Dictionary<int, string> ConnectorTypes = new()
        {
            { (int)ConnectorTypeEnum.FileForwarding, "File Forwarding" },
            { (int)ConnectorTypeEnum.OcrExtraction, "OCR Extraction" },
        };

    }
}