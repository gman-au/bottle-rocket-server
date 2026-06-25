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
using Rocket.Api.Contracts.Dashboard;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/dashboard")]
    [Authorize]
    public class DashboardController(
        ILogger<DashboardController> logger,
        IDashboardSnapshotProvider dashboardSnapshotProvider,
        IUserManager userManager
    ) : RocketControllerBase(userManager)
    {
        [HttpGet]
        [EndpointSummary("Get a dashboard snapshot")]
        [EndpointGroupName("Status")]
        [EndpointDescription(
            """
            Retrieves a snapshot of the current Bottle Rocket dashboard for the given user.\n
            Includes metrics on workflows, scans, and drive space.
            """
        )]
        [ProducesResponseType(
            typeof(FetchDashboardResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDashboardSnapshotAsync(
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
                    "Received dashboard snapshot request for username: {userId}",
                    userId
                );

            var snapshot =
                await
                    dashboardSnapshotProvider
                        .GetSnapshotForUserAsync(
                            userId,
                            cancellationToken
                        );

            var response =
                new FetchDashboardResponse
                {
                    Scans =
                        new ScansResponse
                        {
                            TotalScansReceived = snapshot?.Scans?.TotalScansReceived,
                            ScansReceivedByVendor =
                                (snapshot?.Scans?.ScansReceivedByVendor ?? [])
                                .Select(
                                    o => new ScanByVendorTotalSpecifics
                                    {
                                        Vendor = o.Vendor,
                                        Scans = o.Scans
                                    }
                                )
                        },
                    Storage = new StorageResponse
                    {
                        UsedStorageMb = snapshot?.Storage?.UsedStorageMb,
                        AvailableStorageMb = snapshot?.Storage?.AvailableStorageMb
                    },
                    Executions = new ExecutionsResponse
                    {
                        TotalExecutions = snapshot?.Executions?.TotalExecutions,
                        ExecutionsByStatus =
                            (snapshot?.Executions?.ExecutionsByStatus ?? [])
                            .Select(
                                o => new ExecutionByStatusTotalSpecifics
                                {
                                    Status = Enum.GetName(typeof(ExecutionStatusEnum), o.Status),
                                    Executions = o.Executions
                                }
                            ),
                        ExecutionsByWorkflow =
                            (snapshot?.Executions?.ExecutionsByWorkflow ?? [])
                            .Select(
                                o => new ExecutionByWorkflowTotalSpecifics
                                {
                                    Workflow = o.Workflow,
                                    Executions = o.Executions
                                }
                            ),
                    }
                };

            return
                response
                    .AsApiSuccess();
        }
    }
}