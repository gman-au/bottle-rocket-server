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
        public async Task<IActionResult> CreateWorkflowStepAsync(
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

            // serialize to type
            var response = 
                workflowStep
                    .MapWorkflowStepToSpecific();
            
            return
                response
                    .AsApiSuccess();
        }
    }
}