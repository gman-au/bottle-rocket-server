using System;
using System.Text.Json;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Google.Infrastructure
{
    public class GoogleConnectorMapper(
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<GoogleConnector, GoogleConnectorSpecifics>(serviceProvider)
    {
        public override GoogleConnector For(GoogleConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            var base64String = 
                value
                    .CredentialsFileBase64;
            
            var credentialsJson = 
                Convert
                    .FromBase64String(base64String);

            try
            {
                var credential =
                    JsonSerializer
                        .Deserialize<GooglesCredential>(credentialsJson);

                if (string.IsNullOrEmpty(credential?.ProjectId))
                    throw new RocketException(
                        "There was a problem loading the credentials file. Please check it again and re-try.",
                        ApiStatusCodeEnum.ValidationError
                    );
                
                result.Credential = credential;
            }
            catch (JsonException)
            {
                throw new RocketException(
                    "There was a problem loading the credentials file. Please check it again and re-try.",
                    ApiStatusCodeEnum.ValidationError
                );
            }

            return result;
        }

        public override GoogleConnectorSpecifics From(GoogleConnector value)
        {
            var result =
                base
                    .From(value);
            
            // don't return the credentials

            return result;
        }

        public override async Task PreUpdateAsync(GoogleConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.CredentialsFileBase64))
                throw new RocketException(
                    "No credentials file was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}