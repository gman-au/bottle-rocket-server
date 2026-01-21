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
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/workflowSteps")]
    [Authorize]
    public class WorkflowStepController(
        ILogger<WorkflowStepController> logger,
        IUserManager userManager,
        IWorkflowStepModelMapperRegistry workflowStepModelMapperRegistry,
        IWorkflowStepRepository workflowStepRepository
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("delete")]
        [EndpointSummary("Delete a workflow step")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            Deletes a user's workflow step by its unique ID.
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
        public async Task<IActionResult> DeleteWorkflowStepAsync(
            [FromBody] DeleteWorkflowStepRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            var userId =
                user
                    .Id;

            logger
                .LogInformation(
                    "Received workflow step deletion request for username: {userId}, id: {id}",
                    userId,
                    request.WorkflowStepId
                );

            var result =
                await
                    workflowStepRepository
                        .DeleteWorkflowStepAsync(
                            userId,
                            request.WorkflowId,
                            request.WorkflowStepId,
                            cancellationToken
                        );

            var response =
                new DeleteWorkflowResponse
                {
                    IsDeleted = result
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpGet("{workflowId}/get/{workflowStepId}")]
        [EndpointSummary("Get a specific workflow step")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            TODO
            """
        )]
        [ProducesResponseType(
            typeof(CreateWorkflowResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetWorkflowStepAsync(
            string workflowId,
            string workflowStepId,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received workflow step creation request for username: {username}",
                    user.Username
                );

            var userId = user.Id;

            var workflowStep =
                await
                    workflowStepRepository
                        .GetWorkflowStepByIdAsync(
                            workflowStepId,
                            workflowId,
                            userId,
                            cancellationToken
                        );

            if (workflowStep == null)
                throw new RocketException(
                    "Workflow step could not be found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var mapper =
                workflowStepModelMapperRegistry
                    .GetMapperForDomain(workflowStep.GetType());

            var response =
                mapper
                    .From(workflowStep);

            return
                response
                    .AsApiSuccess();
        }

        [HttpPost("create")]
        [EndpointSummary("Add a new Dropbox workflow step")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            // TODO
            """
        )]
        [ProducesResponseType(
            typeof(CreateWorkflowStepResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateWorkflowStepAsync(
            [FromBody] CreateWorkflowStepRequest<WorkflowStepSummary> request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received workflow step creation request for username: {username}",
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

            var step =
                request
                    .Step;

            var mapper =
                workflowStepModelMapperRegistry
                    .GetMapperForView(step.GetType());

            var newWorkflowStep =
                mapper
                    .For(step);

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

        [HttpPatch("update")]
        [EndpointSummary("Update a workflow step")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            // TODO
            """
        )]
        [ProducesResponseType(
            typeof(UpdateWorkflowResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateWorkflowStepAsync(
            [FromBody] UpdateWorkflowStepRequest<WorkflowStepSummary> request,
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
                request
                    .Step;

            var mapper =
                workflowStepModelMapperRegistry
                    .GetMapperForView(step.GetType());

            var updatedWorkflowStep =
                mapper
                    .For(step);

            updatedWorkflowStep.Id = request.WorkflowStepId;

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
                    "Failed to update workflow step",
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