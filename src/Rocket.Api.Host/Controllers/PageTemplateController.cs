using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.PageTemplates;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/pageTemplates")]
    public class PageTemplateController(
        ILogger<PageTemplateController> logger,
        IUserManager userManager,
        IRocketbookPageTemplateRepository rocketbookPageTemplateRepository
    ) : RocketControllerBase(userManager)
    {
        [HttpGet]
        [EndpointSummary("Fetch the page templates")]
        [EndpointGroupName("Manage page templates")]
        [EndpointDescription(
            """
            // TODO
            """
        )]
        [ProducesResponseType(
            typeof(FetchPageTemplatesResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchPageTemplatesAsync(
            CancellationToken cancellationToken
        )
        {
            var records =
                await
                    rocketbookPageTemplateRepository
                        .FetchAllAsync(
                            cancellationToken
                        );

            var response = new FetchPageTemplatesResponse
            {
                Templates =
                    records
                        .Select(
                            o =>
                                new PageTemplateSummary
                                {
                                    QrCode = o.QrCode,
                                    BookVendor = DomainConstants.BookVendorTypes[(int)BookVendorTypeEnum.Rocketbook]
                                }
                        )
            };

            return
                response
                    .AsApiSuccess();
        }
    }
}