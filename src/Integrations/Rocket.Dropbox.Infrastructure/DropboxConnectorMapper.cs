using System;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Dropbox.Contracts;
using Rocket.Infrastructure.Mapping;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxConnectorMapper(
        IServiceProvider serviceProvider,
        IObfuscator obfuscator,
        IDropboxClientManager dropboxClientManager
    )
        : ConnectorModelMapperBase<DropboxConnector, DropboxConnectorSpecifics>(serviceProvider)
    {
        public override DropboxConnector For(DropboxConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.AppKey = value.AppKey;
            result.AppSecret = value.AppSecret;
            result.RefreshToken = value.RefreshToken;

            return result;
        }

        public override DropboxConnectorSpecifics From(DropboxConnector value)
        {
            var result =
                base
                    .From(value);

            result.ConnectorName = value.ConnectorName;
            result.AppKey = obfuscator.Obfuscate(value.AppKey);
            result.AppSecret = obfuscator.Obfuscate(value.AppSecret);
            result.RefreshToken = obfuscator.Obfuscate(value.RefreshToken);

            return result;
        }

        public override async Task PreUpdateAsync(DropboxConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.AppKey))
                throw new RocketException(
                    "No app key was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            if (string.IsNullOrEmpty(value.AppSecret))
                throw new RocketException(
                    "No app secret was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }

        public override async Task<ApiResponse> PostUpdateAsync(DropboxConnector value)
        {
            var authorizeUri =
                dropboxClientManager
                    .GetAuthorizeUrl(value.AppKey);

            return new CreateConnectorResponse<DropboxConnectorSpecifics>
            {
                Id = value.Id,
                Connector = new DropboxConnectorSpecifics
                {
                    AuthorizeUri = authorizeUri
                }
            };
        }
    }
}