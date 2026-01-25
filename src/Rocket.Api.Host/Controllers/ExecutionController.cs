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
        IExecutionScheduler executionScheduler,
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
            Retrieves a subset of executions belonging to the authenticated user.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load.\n
            Optionally, a workflow ID and scan ID can be supplied as additional filters.
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
                            request.ScanId,
                            request.WorkflowId,
                            cancellationToken
                        );

            var response =
                new FetchExecutionsResponse
                {
                    Executions =
                        records
                            .Select(
                                o =>
                                    new ExecutionSummary
                                    {
                                        Id = o.Id,
                                        UserId = o.UserId,
                                        ScanId = o.ScanId,
                                        WorkflowId = o.WorkflowId,
                                        MatchingPageSymbol = o.MatchingPageSymbol,
                                        CreatedAt = o.CreatedAt,
                                        RunDate = o.RunDate,
                                        Name = o.Name,
                                        ExecutionStatus = o.ExecutionStatus,
                                        ThumbnailBase64 = o.ThumbnailBase64,
                                        ContentType = o.ContentType
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
            Deletes a user's workflow execution by its unique ID.
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
            Creates a new workflow execution for the given user, workflow ID, and captured scan ID.\n
            Throws an error if the any of the related objects (user, scan, workflow) cannot be found for the given user.\n
            If the run immediately flag is set to true, the execution will be scheduled to run immediately.\n
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

            var executionId =
                await
                    executionScheduler
                        .ScheduleExecutionAsync(
                            request.WorkflowId,
                            request.ScanId,
                            userId,
                            request.RunImmediately ?? false,
                            cancellationToken
                        );

            var response =
                new CreateExecutionResponse
                {
                    Id = executionId
                };

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
        [EndpointDescription("Attempts to start an inactive workflow execution by its unique identifier.")]
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
                        .StartExecutionAsync(
                            id,
                            userId,
                            cancellationToken
                        );

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
                    ScanId = execution.ScanId,
                    WorkflowId = execution.WorkflowId,
                    MatchingPageSymbol = execution.MatchingPageSymbol,
                    RunDate = execution.RunDate,
                    CreatedAt = execution.CreatedAt,
                    Name = execution.Name,
                    ExecutionStatus = execution.ExecutionStatus,
                    ThumbnailBase64 = execution.ThumbnailBase64,
                    ContentType = execution.ContentType,
                    Steps =
                        (execution.Steps ?? [])
                        .Select(
                            o =>
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