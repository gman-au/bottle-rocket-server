using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Web.Host.Infrastructure
{
    public class TemplateDownloader(
        IImageBase64Converter imageBase64Converter,
        IWebHostEnvironment environment,
        IWebHostErrorHandler webHostErrorHandler,
        IJSRuntime jsRuntime
    ) : ITemplateDownloader
    {
        private static readonly Dictionary<int, string> Templates =
            new()
            {
                { (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData, "Project Tasks Template.pdf" },
                { (int)WorkflowFormatTypeEnum.WeeklyPlanData, "Weekly Planner Template.pdf" }
            };

        public bool HasATemplate(
            int workflowType
        ) => Templates.ContainsKey(workflowType);

        public async Task GetTemplateForWorkflowTypeAsync(
            int workflowType
        )
        {
            try
            {
                if (!HasATemplate(workflowType))
                    throw new RocketException(
                        "No template found for workflow type",
                        ApiStatusCodeEnum.ValidationError
                    );

                var fileName = Templates[workflowType];

                var filePath =
                    Path.Combine(
                        environment
                            .WebRootPath,
                        "pdf",
                        fileName
                    );

                if (File.Exists(filePath))
                {
                    var fileBytes =
                        await
                            File
                                .ReadAllBytesAsync(filePath);

                    var contentType = "pdf";

                    var base64Data =
                        imageBase64Converter
                            .Perform(fileBytes);

                    if (!string.IsNullOrEmpty(base64Data))
                    {
                        if (base64Data.Contains(","))
                        {
                            base64Data = 
                                base64Data
                                    .Split(',')[1];
                        }

                        await
                            jsRuntime
                                .InvokeVoidAsync(
                                    "downloadBase64File",
                                    fileName,
                                    contentType,
                                    base64Data
                                );
                    }
                }
            }
            catch (Exception ex)
            {
                await
                    webHostErrorHandler
                        .HandleExceptionAsync(ex);
            }
        }
    }
}