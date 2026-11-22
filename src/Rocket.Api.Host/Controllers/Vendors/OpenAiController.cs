using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;
using Rocket.OpenAi.Contracts;
using Rocket.OpenAi.Domain;
using Rocket.OpenAi.Infrastructure;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Authorize]
    public class OpenAiController(
        ILogger<OpenAiController> logger,
        IConnectorRepository connectorRepository,
        IOpenAiClient openAiClient,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpPost, Route("/api/openai/models")]
        [EndpointSummary("List available OpenAI models and their capabilities")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Attempts to retrieve a list of installed models on the given OpenAI connector.\n
            If successful, the OpenAI server should return a list of models and their capabilities.
            """
        )]
        [ProducesResponseType(
            typeof(OpenAiListModelsResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InitiateAuthRequestAsync(
            [FromBody] OpenAiListModelsRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received OpenAI list models request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var connectorId = request.ConnectorId;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<OpenAiConnector>(
                            userId,
                            connectorId,
                            cancellationToken
                        );

            if (connector == null)
                throw new RocketException(
                    "Connector entry not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var apiKey =
                connector.ApiKey ??
                throw new ConfigurationErrorsException(
                    nameof(connector.ApiKey)
                );

            var modelList =
                (await
                    openAiClient
                        .GetModelListAsync(
                            apiKey,
                            cancellationToken
                        ))
                .ToList();

            return
                new OpenAiListModelsResponse
                    {
                        Models =
                            modelList
                                .Select(o =>
                                    new OpenAiModel
                                    {
                                        Name = o.Id
                                    }
                                )
                    }
                    .AsApiSuccess();
        }
    }
}