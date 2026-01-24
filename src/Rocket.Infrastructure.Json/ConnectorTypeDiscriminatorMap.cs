using System;
using System.Collections.Generic;
using Rocket.Dropbox.Contracts;
using Rocket.MaxOcr.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class ConnectorTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxConnectorSpecifics), "dropbox_connector" },
            { typeof(MaxOcrConnectorSpecifics), "maxocr_connector" }
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