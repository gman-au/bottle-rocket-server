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
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/workflowSteps")]
    [Authorize]
    public class WorkflowStepController(
        ILogger<WorkflowStepController> logger,
        IUserManager userManager,
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
                    request.Id
                );

            var result =
                await
                    workflowStepRepository
                        .DeleteWorkflowStepAsync(
                            userId,
                            request.WorkflowId,
                            request.Id,
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

        [HttpPost("create")]
        [EndpointSummary("Add a new workflow step")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            Creates a new workflow step for the given workflow. Validates that the output and input types will match.
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
        public async Task<IActionResult> CreateWorkflowStepAsync(
            [FromBody] CreateWorkflowStepRequest request,
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
            var workflowId = request.WorkflowId;

            var workflow =
                await
                    workflowStepRepository
                        .GetWorkflowForUserByIdAsync(
                            workflowId,
                            userId,
                            cancellationToken
                        );

            if (workflow == null)
                throw new RocketException(
                    "Workflow could not be found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var newWorkflowStep =
                new DropboxUploadStep();

            var parentStepId = request.ParentStepId;

            var result =
                await
                    workflowStepRepository
                        .InsertWorkflowStepAsync(
                            newWorkflowStep,
                            userId,
                            workflowId,
                            parentStepId,
                            cancellationToken
                        );

            if (result == null)
                throw new RocketException(
                    "Failed to create workflow",
                    ApiStatusCodeEnum.ServerError
                );

            var response = new CreateWorkflowResponse
            {
                Id = result.Id
            };

            return
                response
                    .AsApiSuccess();
        }
    }
}