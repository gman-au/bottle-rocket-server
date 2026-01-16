using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Workflows;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Connectors;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Utils;
using Rocket.Dropbox.Contracts;
using Rocket.Dropbox.Infrastructure;
using Rocket.Interfaces;
using DropboxUploadStep = Rocket.Domain.Workflows.DropboxUploadStep;

namespace Rocket.Api.Host.Controllers.Vendors
{
    [ApiController]
    [Authorize]
    public class DropboxController(
        ILogger<DropboxController> logger,
        IUserManager userManager,
        IDropboxClientManager dropboxClientManager,
        IConnectorRepository connectorRepository,
        IWorkflowStepRepository workflowStepRepository
    ) : RocketControllerBase(userManager)
    {
        [HttpPost, Route("/api/dropbox/connectors/create")]
        [EndpointSummary("Add a new Dropbox connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Creates a new Dropbox connector for the given user. Will return an error if the same named
            connector already exists for the given user.
            """
        )]
        [ProducesResponseType(
            typeof(CreateDropboxConnectorResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateConnectorAsync(
            [FromBody] CreateDropboxConnectorRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Dropbox) connector creation request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            if (await
                connectorRepository
                    .ConnectorExistsForUserAsync(
                        userId,
                        DomainConstants.VendorDropbox,
                        cancellationToken
                    )
               )
                throw new RocketException(
                    "Dropbox connector already exists",
                    ApiStatusCodeEnum.RecordAlreadyExists
                );

            if (string.IsNullOrEmpty(request.AppKey))
                throw new RocketException(
                    "No app key was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            if (string.IsNullOrEmpty(request.AppSecret))
                throw new RocketException(
                    "No app secret was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            var newConnector =
                new DropboxConnector
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    AppKey = request.AppKey,
                    AppSecret = request.AppSecret
                };

            var result =
                await
                    connectorRepository
                        .InsertConnectorAsync(
                            newConnector,
                            cancellationToken
                        );

            if (result == null)
                throw new RocketException(
                    "Failed to create Dropbox connector",
                    ApiStatusCodeEnum.ServerError
                );

            var authorizeUri =
                dropboxClientManager
                    .GetAuthorizeUrl(request.AppKey);

            var response =
                new CreateDropboxConnectorResponse
                {
                    Id = result.Id,
                    AuthorizeUri = authorizeUri
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPatch, Route("/api/dropbox/connectors/finalize")]
        [EndpointSummary("Finalize a Dropbox connector")]
        [EndpointGroupName("Manage connectors")]
        [EndpointDescription(
            """
            Finalizes a Dropbox connector for the given user.
            """
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FinalizeConnectorAsync(
            [FromBody] FinalizeDropboxConnectorRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Dropbox) connector update request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var id = request.Id;

            var connector =
                await
                    connectorRepository
                        .GetConnectorByIdAsync<DropboxConnector>(
                            userId,
                            id,
                            cancellationToken
                        );

            if (connector == null)
                throw new RocketException(
                    "Connector entry not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var appKey = connector.AppKey;
            var appSecret = connector.AppSecret;
            var accessCode = request.AccessCode;

            var refreshToken =
                await
                    dropboxClientManager
                        .GetRefreshTokenFromAccessCodeAsync(
                            appKey,
                            appSecret,
                            accessCode
                        );

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await
                    connectorRepository
                        .UpdateConnectorFieldAsync<DropboxConnector, string>(
                            id,
                            userId,
                            o =>
                                o.RefreshToken,
                            refreshToken,
                            cancellationToken
                        );
            }

            var lastUpdatedAt = DateTime.UtcNow;

            await
                connectorRepository
                    .UpdateConnectorFieldAsync<DropboxConnector, DateTime?>(
                        id,
                        userId,
                        o =>
                            o.LastUpdatedAt,
                        lastUpdatedAt,
                        cancellationToken
                    );

            return
                new ApiResponse()
                    .AsApiSuccess();
        }

        [HttpPost, Route("/api/dropbox/workflowSteps/create")]
        [EndpointSummary("Add a new Dropbox workflow step")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            // TODO
            """
        )]
        [ProducesResponseType(
            typeof(CreateDropboxConnectorResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateUploadFileWorkflowStepAsync(
            [FromBody] CreateWorkflowStepRequest<DropboxUploadStepSpecifics> request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Dropbox) workflow step creation request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            if (string.IsNullOrEmpty(request.WorkflowId))
                throw new RocketException(
                    "No workflow ID was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            if (request.Step == null)
                throw new RocketException(
                    "Could not determine workflow step from request",
                    ApiStatusCodeEnum.ValidationError
                );

            var newWorkflowStep =
                new DropboxUploadStep
                {
                    ConnectionId = request.Step.ConnectionId,
                    Subfolder = request.Step.Subfolder,
                };

            var result =
                await
                    workflowStepRepository
                        .InsertWorkflowStepAsync(
                            newWorkflowStep,
                            userId,
                            request.WorkflowId,
                            request.ParentStepId,
                            cancellationToken
                        );

            if (result == null)
                throw new RocketException(
                    "Failed to create Dropbox workflow step",
                    ApiStatusCodeEnum.ServerError
                );

            var response =
                new CreateWorkflowStepResponse
                {
                    Id = result.Id
                };

            return
                response
                    .AsApiSuccess();
        }
        
        [HttpPatch, Route("/api/dropbox/workflowSteps/update")]
        [EndpointSummary("Add a new Dropbox workflow step")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            // TODO
            """
        )]
        [ProducesResponseType(
            typeof(CreateDropboxConnectorResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUploadFileWorkflowStepAsync(
            [FromBody] UpdateWorkflowStepRequest<DropboxUploadStepSpecifics> request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received (Dropbox) workflow step update request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            if (string.IsNullOrEmpty(request.WorkflowId))
                throw new RocketException(
                    "No workflow ID was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
            
            var step =
                await
                    workflowStepRepository
                        .GetWorkflowStepByIdAsync(
                            request.WorkflowStepId,
                            request.WorkflowId,
                            userId,
                            cancellationToken
                        );

            if (step == null)
                throw new RocketException(
                    "Could not find workflow step from request",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var updatedWorkflowStep =
                new DropboxUploadStep
                {
                    Id = step.Id,
                    ChildSteps = step.ChildSteps,
                    ConnectionId = request.Step.ConnectionId,
                    Subfolder = request.Step.Subfolder,
                };

            var result =
                await
                    workflowStepRepository
                        .UpdateWorkflowStepAsync(
                            request.WorkflowStepId,
                            request.WorkflowId,
                            userId,
                            updatedWorkflowStep,
                            cancellationToken
                        );

            if (result == null)
                throw new RocketException(
                    "Failed to update Dropbox workflow step",
                    ApiStatusCodeEnum.ServerError
                );

            var response =
                new UpdateWorkflowStepResponse
                {
                    Id = result.Id
                };

            return
                response
                    .AsApiSuccess();
        }
    }
}