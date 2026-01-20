using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Executions;
using Rocket.Api.Contracts.Users;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/executions")]
    [Authorize]
    public class ExecutionController(
        ILogger<ExecutionController> logger,
        IUserManager userManager,
        IWorkflowRepository workflowRepository,
        IWorkflowCloner workflowCloner,
        IWorkflowExecutionManager workflowExecutionManager,
        IExecutionRepository executionRepository,
        IExecutionStepModelMapperRegistry executionStepModelMapperRegistry
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("fetch")]
        [EndpointSummary("Fetch the user workflow executions")]
        [EndpointGroupName("Manage executions")]
        [EndpointDescription(
            """
            // TODO 
            """
        )]
        [ProducesResponseType(
            typeof(FetchExecutionsResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchWorkflowsAsync(
            [FromBody] FetchExecutionsRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            var userId =
                user
                    .Id;

            var (records, totalRecordCount) =
                await
                    executionRepository
                        .FetchExecutionsAsync(
                            userId,
                            request.StartIndex,
                            request.RecordCount,
                            cancellationToken
                        );

            var response =
                new FetchExecutionsResponse
                {
                    Executions =
                        records
                            .Select(o =>
                                new ExecutionSummary
                                {
                                    Id = o.Id,
                                    UserId = o.UserId,
                                    MatchingPageSymbol = o.MatchingPageSymbol,
                                    CreatedAt = o.CreatedAt,
                                    RunDate = o.RunDate,
                                    Name = o.Name,
                                    ExecutionStatus = o.ExecutionStatus
                                }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete an execution")]
        [EndpointGroupName("Manage executions")]
        [EndpointDescription(
            """
            Deletes a user's workflow execution by its unique ID.\n
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
        public async Task<IActionResult> DeleteExecutionAsync(
            string id,
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
                    "Received workflow execution deletion request for username: {userId}, id: {id}",
                    userId,
                    id
                );

            var result =
                await
                    executionRepository
                        .DeleteExecutionAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            var response =
                new DeleteExecutionResponse
                {
                    IsDeleted = result
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPost("create")]
        [EndpointSummary("Add a new execution")]
        [EndpointGroupName("Manage executions")]
        [EndpointDescription(
            """
            Creates a new workflow execution for the given user. Throws an error if the workflow ID cannot be found for the given user.
            """
        )]
        [ProducesResponseType(
            typeof(CreateExecutionResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateExecutionAsync(
            [FromBody] CreateExecutionRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received workflow creation request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var workflow =
                await
                    workflowRepository
                        .GetWorkflowByIdAsync(
                            userId,
                            request.WorkflowId,
                            cancellationToken
                        );

            if (workflow == null)
                throw new RocketException(
                    "Workflow does not exist for this user.",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );

            var newExecution =
                workflowCloner
                    .Clone(workflow);

            var result =
                await
                    executionRepository
                        .InsertExecutionAsync(
                            newExecution,
                            cancellationToken
                        );

            if (result == null)
                throw new RocketException(
                    "Failed to create execution",
                    ApiStatusCodeEnum.ServerError
                );

            var response = new CreateExecutionResponse
            {
                Id = result.Id
            };

            // trigger background queue job
            if (request.RunImmediately.GetValueOrDefault())
            {
                await
                    workflowExecutionManager
                        .StartExecutionAsync(result.Id);
            }

            return
                response
                    .AsApiSuccess();
        }

        [HttpPut("cancel/{id}")]
        [EndpointSummary("Cancel a (running) execution by ID")]
        [EndpointGroupName("Manage executions")]
        [EndpointDescription("Attempts to cancel a running workflow execution by its unique identifier.")]
        [ProducesResponseType(
            typeof(UserSpecifics),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CancelExecutionAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received cancel execution request for id: {id}",
                    id
                );

            var userId =
                user
                    .Id;

            var execution =
                await
                    executionRepository
                        .GetExecutionByIdAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            if (execution == null)
            {
                throw new RocketException(
                    $"Execution id: {id} not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );
            }

            var result =
                await
                    workflowExecutionManager
                        .CancelExecutionAsync(id);

            var response =
                new CancelExecutionResponse
                {
                    IsCancelled = result
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPut("start/{id}")]
        [EndpointSummary("Start an inactive execution by ID")]
        [EndpointGroupName("Manage executions")]
        [EndpointDescription("Attempts to start a running workflow execution by its unique identifier.")]
        [ProducesResponseType(
            typeof(UserSpecifics),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> StartExecutionAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received start execution request for id: {id}",
                    id
                );

            var userId =
                user
                    .Id;

            var execution =
                await
                    executionRepository
                        .GetExecutionByIdAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            if (execution == null)
            {
                throw new RocketException(
                    $"Execution id: {id} not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );
            }

            var result =
                await
                    workflowExecutionManager
                        .StartExecutionAsync(id);

            var response =
                new StartExecutionResponse
                {
                    IsStarted = result
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpGet("get/{id}")]
        [EndpointSummary("Get execution by ID")]
        [EndpointGroupName("Manage executions")]
        [EndpointDescription("Returns a execution by its unique identifier.")]
        [ProducesResponseType(
            typeof(UserSpecifics),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetExecutionAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received execution request for id: {id}",
                    id
                );

            var userId =
                user
                    .Id;

            var execution =
                await
                    executionRepository
                        .GetExecutionByIdAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            if (execution == null)
            {
                throw new RocketException(
                    $"Execution id: {id} not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );
            }

            var response =
                new ExecutionSummary
                {
                    Id = execution.Id,
                    UserId = execution.UserId,
                    MatchingPageSymbol = execution.MatchingPageSymbol,
                    RunDate = execution.RunDate,
                    CreatedAt = execution.CreatedAt,
                    Name = execution.Name,
                    ExecutionStatus = execution.ExecutionStatus,
                    Steps =
                        (execution.Steps ?? [])
                        .Select(o =>
                            {
                                var mapper =
                                    executionStepModelMapperRegistry
                                        .GetMapperForDomain(o.GetType());

                                return
                                    mapper
                                        .From(o);
                            }
                        )
                };

            return
                response
                    .AsApiSuccess();
        }
    }
}