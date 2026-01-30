using System;
using System.Collections.Generic;
using Rocket.Dropbox.Contracts;
using Rocket.Gcp.Contracts;
using Rocket.MaxOcr.Contracts;
using Rocket.Microsofts.Contracts;
using Rocket.Notion.Contracts;
using Rocket.Ollama.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class ConnectorTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxConnectorSpecifics), "dropbox_connector" },
            { typeof(MaxOcrConnectorSpecifics), "maxocr_connector" },
            { typeof(OllamaConnectorSpecifics), "ollama_connector" },
            { typeof(GcpConnectorSpecifics), "gcp_connector" },
            { typeof(NotionConnectorSpecifics), "notion_connector" },
            { typeof(MicrosoftConnectorSpecifics), "microsoft_connector" }
        };

        public static string GetConnectorTypeDiscriminator(this Type type)
        {
            return
                TypeDiscriminatorMap
                    .GetValueOrDefault(
                        type,
                        "base"
                    );
        }
    }
}