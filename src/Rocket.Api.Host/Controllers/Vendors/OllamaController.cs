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
using Rocket.Ollama.Contracts;
using Rocket.Ollama.Domain;
using Rocket.Ollama.Infrastructure;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Authorize]
    public class OllamaController(
        ILogger<OllamaController> logger,
        IConnectorRepository connectorRepository,
        IOllamaClient ollamaClient,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpPost, Route("/api/ollama/models")]
        [EndpointSummary("List available Ollama models and their capabilities")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Attempts to retrieve a list of installed models on the given Ollama connector.\n
            If successful, the Ollama server should return a list of models and their capabilities.
            """
        )]
        [ProducesResponseType(
            typeof(OllamaListModelsResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> InitiateAuthRequestAsync(
            [FromBody] OllamaListModelsRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received Ollama list models request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var connectorId = request.ConnectorId;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<OllamaConnector>(
                            userId,
                            connectorId,
                            cancellationToken
                        );

            if (connector == null)
                throw new RocketException(
                    "Connector entry not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var endpoint =
                connector.Endpoint ??
                throw new ConfigurationErrorsException(
                    nameof(connector.Endpoint)
                );

            var modelList =
                (await
                    ollamaClient
                        .GetModelListAsync(
                            endpoint,
                            cancellationToken
                        ))
                .ToList();

            return
                new OllamaListModelsResponse
                    {
                        Models =
                            modelList
                                .Select(o =>
                                    new OllamaModel
                                    {
                                        Name = o.Name,
                                        Model = o.Model,
                                        Capabilities = o.Capabilities
                                    }
                                )
                    }
                    .AsApiSuccess();
        }
    }
}