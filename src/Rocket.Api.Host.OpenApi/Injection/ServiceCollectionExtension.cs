using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Rocket.Api.Host.OpenApi.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
        {
            services
                .AddOpenApi(
                    options =>
                    {
                        options.ShouldInclude = _ => true;

                        // Use an Operation Transformer to copy the GroupName into the Tags list
                        options
                            .AddOperationTransformer(
                                (operation, context, _) =>
                                {
                                    // Search for the EndpointGroupNameAttribute in the endpoint metadata
                                    var groupNameMetadata =
                                        context
                                            .Description
                                            .ActionDescriptor
                                            .EndpointMetadata
                                            .OfType<EndpointGroupNameAttribute>()
                                            .FirstOrDefault();

                                    if (groupNameMetadata != null)
                                    {
                                        operation
                                            .Tags
                                            .Clear();

                                        operation
                                            .Tags
                                            .Add(
                                                new OpenApiTag
                                                {
                                                    Name = groupNameMetadata.EndpointGroupName
                                                }
                                            );
                                    }

                                    if (!string.IsNullOrEmpty(operation.Description))
                                    {
                                        operation.Description =
                                            operation
                                                .Description
                                                .Replace(
                                                    "\\n",
                                                    "\n"
                                                );
                                    }

                                    return
                                        Task
                                            .CompletedTask;
                                }
                            );

                        options.AddDocumentTransformer(
                            (document, context, cancellationToken) =>
                            {
                                document.Info.Title = "Bottle Rocket API";
                                document.Info.Version = "##{VERSION_TAG}##";
                                document.Tags = null;

                                return Task.CompletedTask;
                            }
                        );
                    }
                );

            return services;
        }
    }
}