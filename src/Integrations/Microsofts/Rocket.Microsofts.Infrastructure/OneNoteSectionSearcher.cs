using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneNoteSectionSearcher(IGraphClientProvider graphClientProvider) : IOneNoteSectionSearcher
    {
        public async Task<IEnumerable<OneNoteSection>> GetSectionsAsync(
            MicrosoftConnector connector,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var graphClient =
                    await
                        graphClientProvider
                            .GetClientAsync(
                                connector,
                                cancellationToken
                            );

                var sections =
                    await
                        graphClient
                            .Me
                            .Onenote
                            .Sections
                            .Request()
                            .GetAsync(cancellationToken);

                var results =
                    sections
                        .CurrentPage
                        .Select(
                            o =>
                                new OneNoteSection
                                {
                                    Id = o.Id,
                                    SectionName = o.DisplayName,
                                }
                        );

                return results;
            }
            catch (ServiceException ex)
            {
                throw new RocketException(
                    "There was an error connecting to the Microsoft service.",
                    ApiStatusCodeEnum.ThirdPartyServiceError,
                    innerException: ex
                );
            }
        }
    }
}