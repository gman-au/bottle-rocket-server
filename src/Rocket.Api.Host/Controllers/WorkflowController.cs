using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Users;
using Rocket.Api.Contracts.Workflows;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/workflows")]
    [Authorize]
    public class WorkflowController(
        ILogger<WorkflowController> logger,
        IUserManager userManager,
        IWorkflowRepository workflowRepository,
        IWorkflowStepModelMapperRegistry workflowStepModelMapperRegistry
    ) : RocketControllerBase(userManager)
    {
        [HttpPost("fetch")]
        [EndpointSummary("Fetch the user workflows")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            Retrieves a subset of workflows belonging to the authenticated user.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load. 
            """
        )]
        [ProducesResponseType(
            typeof(FetchWorkflowsResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchWorkflowsAsync(
            [FromBody] FetchWorkflowsRequest request,
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
                    workflowRepository
                        .FetchWorkflowsAsync(
                            userId,
                            request.StartIndex,
                            request.RecordCount,
                            cancellationToken
                        );

            var response =
                new FetchWorkflowsResponse
                {
                    Workflows =
                        records
                            .Select(o =>
                                new MyWorkflowSummary
                                {
                                    Id = o.Id,
                                    MatchingPageSymbol = o.MatchingPageSymbol,
                                    Name = o.Name,
                                    IsActive = o.IsActive,
                                    CreatedAt = o.CreatedAt.ToLocalTime(),
                                    LastUpdatedAt = o.LastUpdatedAt?.ToLocalTime(),
                                }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete a workflow")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            Deletes a user's workflow by its unique ID.
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
        public async Task<IActionResult> DeleteWorkflowAsync(
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
                    "Received workflow deletion request for username: {userId}, id: {id}",
                    userId,
                    id
                );

            var result =
                await
                    workflowRepository
                        .DeleteWorkflowAsync(
                            userId,
                            id,
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
        [EndpointSummary("Add a new workflow")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            Creates a new workflow for the given user. Will return an error if the same named
            workflow already exists for the given user.
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
        public async Task<IActionResult> CreateWorkflowAsync(
            [FromBody] CreateWorkflowRequest request,
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

            if (string.IsNullOrEmpty(request.Name))
                throw new RocketException(
                    "No name was provided.",
                    ApiStatusCodeEnum.ValidationError
                );

            if (await
                workflowRepository
                    .WorkflowExistsForNameAsync(
                        userId,
                        request.Name,
                        cancellationToken
                    )
               )
                throw new RocketException(
                    "Workflow already exists with this name.",
                    ApiStatusCodeEnum.RecordAlreadyExists
                );

            if (request.MatchingPageSymbol.HasValue)
            {
                if (await
                    workflowRepository
                        .WorkflowExistsForMatchingSymbolAsync(
                            userId,
                            request.MatchingPageSymbol.Value,
                            cancellationToken
                        )
                   )
                    throw new RocketException(
                        "Workflow already exists with this matching page symbol.",
                        ApiStatusCodeEnum.RecordAlreadyExists
                    );
            }

            var newWorkflow =
                new Workflow
                {
                    UserId = userId,
                    MatchingPageSymbol = request.MatchingPageSymbol,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    Name = request.Name,
                    IsActive = true,
                    Steps = []
                };

            var result =
                await
                    workflowRepository
                        .InsertWorkflowAsync(
                            newWorkflow,
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

        [HttpPatch("update")]
        [EndpointSummary("Update an existing workflow")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription(
            """
            Updates one or more details of an existing workflow. A value not supplied will not be updated.
            Where a matching page symbol is supplied, it will be checked against any existing workflow, and return
            an error if already in use.
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
        public async Task<IActionResult> UpdateWorkflowAsync(
            [FromBody] MyWorkflowSummary request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received workflow update request for username: {username}",
                    user.Username
                );

            var userId =
                user
                    .Id;

            var name = request.Name;

            if (!string.IsNullOrEmpty(name))
            {
                if (await
                    workflowRepository
                        .WorkflowExistsForNameAsync(
                            userId,
                            request.Name,
                            cancellationToken
                        )
                   )
                {
                    throw new RocketException(
                        "Workflow already exists",
                        ApiStatusCodeEnum.RecordAlreadyExists
                    );
                }

                await
                    workflowRepository
                        .UpdateWorkflowFieldAsync(
                            request.Id,
                            userId,
                            o =>
                                o.Name,
                            name,
                            cancellationToken
                        );
            }

            if (request.MatchingPageSymbol.HasValue)
            {
                if (await
                    workflowRepository
                        .WorkflowExistsForMatchingSymbolAsync(
                            userId,
                            request.MatchingPageSymbol.Value,
                            cancellationToken
                        )
                   )
                    throw new RocketException(
                        "Workflow already exists with this matching page symbol.",
                        ApiStatusCodeEnum.RecordAlreadyExists
                    );

                await
                    workflowRepository
                        .UpdateWorkflowFieldAsync(
                            request.Id,
                            userId,
                            o =>
                                o.MatchingPageSymbol,
                            request.MatchingPageSymbol.Value,
                            cancellationToken
                        );
            }

            if (request.IsActive.HasValue)
            {
                await
                    workflowRepository
                        .UpdateWorkflowFieldAsync(
                            request.Id,
                            userId,
                            o =>
                                o.IsActive,
                            request.IsActive.Value,
                            cancellationToken
                        );
            }

            // page symbol is nullable / clearable
            await
                workflowRepository
                    .UpdateWorkflowFieldAsync(
                        request.Id,
                        userId,
                        o =>
                            o.MatchingPageSymbol,
                        request.MatchingPageSymbol,
                        cancellationToken
                    );

            var response =
                new UpdateWorkflowResponse();

            return
                response
                    .AsApiSuccess();
        }

        [HttpGet("get/{id}")]
        [EndpointSummary("Get workflow by ID")]
        [EndpointGroupName("Manage workflows")]
        [EndpointDescription("Returns a workflow by its unique identifier.")]
        [ProducesResponseType(
            typeof(UserSpecifics),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetWorkflowAsync(
            string id,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            logger
                .LogInformation(
                    "Received workflow request for id: {id}",
                    id
                );

            var userId =
                user
                    .Id;

            var workflow =
                await
                    workflowRepository
                        .GetWorkflowByIdAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            if (workflow == null)
            {
                throw new RocketException(
                    $"Workflow id: {id} not found",
                    ApiStatusCodeEnum.UnknownOrInaccessibleRecord
                );
            }

            var response =
                new WorkflowSummary
                {
                    Id = workflow.Id,
                    UserId = workflow.UserId,
                    MatchingPageSymbol = workflow.MatchingPageSymbol,
                    CreatedAt = workflow.CreatedAt.ToLocalTime(),
                    LastUpdatedAt = workflow.LastUpdatedAt?.ToLocalTime(),
                    Name = workflow.Name,
                    IsActive = workflow.IsActive,
                    Steps =
                        (workflow.Steps ?? [])
                        .Select(o =>
                            {
                                var mapper =
                                    workflowStepModelMapperRegistry
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