using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.Notion.Contracts;
using Rocket.Notion.Domain;

namespace Rocket.Notion.Infrastructure
{
    public class NotionConnectorMapper(
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<NotionConnector, NotionConnectorSpecifics>(serviceProvider)
    {
        public override NotionConnector For(NotionConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.IntegrationSecret = value.IntegrationSecret;

            return result;
        }

        public override NotionConnectorSpecifics From(NotionConnector value)
        {
            var result =
                base
                    .From(value);

            result.IntegrationSecret = value.IntegrationSecret;

            return result;
        }

        public override async Task PreUpdateAsync(NotionConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.IntegrationSecret))
                throw new RocketException(
                    "No integration secret was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}