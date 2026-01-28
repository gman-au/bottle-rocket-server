using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Workflows;
using Rocket.Diagnostics.Contracts;
using Rocket.Domain.Enum;
using Rocket.Domain.Utils;
using Rocket.Dropbox.Contracts;
using Rocket.Gcp.Contracts;
using Rocket.Infrastructure.Mermaid.Extensions;
using Rocket.Interfaces;
using Rocket.Microsofts.Contracts;
using Rocket.Notion.Contracts;
using Rocket.Ollama.Contracts;

namespace Rocket.Infrastructure.Mermaid
{
    public class WorkflowMermaidConverter(ILogger<WorkflowMermaidConverter> logger) : IWorkflowMermaidConverter
    {
        private const string StartNode = "Bottle Rocket";
        private const string AddNewStep = "+ Add New Step";
        private const string AddBackgroundButtonColor = "#a8ef6d";

        public string Convert(WorkflowSummary workflow)
        {
            using var aliasEnumerator =
                "a"
                    .ToSequence()
                    .GetEnumerator();

            var result = new StringBuilder();

            aliasEnumerator.MoveNext();
            var parent = aliasEnumerator.Current;

            var entitiesBuilder = new StringBuilder();
            var linksBuilder = new StringBuilder();
            var clicksBuilder = new StringBuilder();
            var styleBuilder = new StringBuilder();

            // start node
            entitiesBuilder
                .AppendLine($"{parent}@{{ shape: lin-rect, label: \"This workflow\"}}");

            var workflowId =
                workflow
                    .Id;

            WriteNested(
                workflowId,
                parent,
                entitiesBuilder,
                linksBuilder,
                clicksBuilder,
                styleBuilder,
                workflow.Steps,
                aliasEnumerator,
                DomainConstants.WorkflowFormatTypes[(int)WorkflowFormatTypeEnum.ImageData],
                null
            );

            aliasEnumerator
                .MoveNext();

            // add the "+ Add Step" option for each child
            entitiesBuilder
                .AppendLine($"{aliasEnumerator.Current}([\"{AddNewStep}\"]):::clickable");

            linksBuilder
                .AppendLine(
                    $"{parent} --> |{DomainConstants.WorkflowFormatTypes[(int)WorkflowFormatTypeEnum.ImageData]}| {aliasEnumerator.Current}"
                );

            clicksBuilder
                .AppendLine(
                    $"click {aliasEnumerator.Current} call blazorNavigateToRoute(\"/MyWorkflow/{workflowId}/AddStep\")"
                );

            styleBuilder
                .AppendLine($"style {aliasEnumerator.Current} fill:{AddBackgroundButtonColor}");

            result
                .AppendLine("flowchart TD")
                .Append(entitiesBuilder)
                .Append(linksBuilder)
                .Append(clicksBuilder)
                .Append(styleBuilder);

            logger
                .LogDebug("mermaid: {mermaid}", result.ToString());

            return
                result
                    .ToString();
        }

        private static void WriteNested(
            string workflowId,
            string currentParentAlias,
            StringBuilder entitiesBuilder,
            StringBuilder linksBuilder,
            StringBuilder clicksBuilder,
            StringBuilder styleBuilder,
            IEnumerable<WorkflowStepSummary> steps,
            IEnumerator<string> aliasEnumerator,
            string currentParentOutputTypeName,
            string currentParentId
        )
        {
            foreach (var step in steps ?? [])
            {
                aliasEnumerator
                    .MoveNext();

                var requiresConnection = !string.IsNullOrEmpty(step.RequiresConnectorCode);
                var missingConnection = string.IsNullOrEmpty(step.ConnectorId);

                var currentChildAlias =
                    aliasEnumerator
                        .Current;

                var entityLine = $"{currentChildAlias}";
                entityLine += "(\"";

                if (requiresConnection)
                {
                    if (missingConnection)
                    {
                        entityLine += "\u26a0\ufe0f";
                    }
                    else
                    {
                        entityLine += "\u2705";
                    }
                }

                entityLine += $" {step.StepName}";
                entityLine += "\")";

                linksBuilder
                    .AppendLine($"{currentParentAlias} --> |{currentParentOutputTypeName}| {currentChildAlias}");

                var route = string.Empty;
                if (step is DropboxUploadWorkflowStepSpecifics)
                {
                    route = $"/MyWorkflow/Dropbox/{workflowId}/Steps/{step.Id}/UpdateStep";
                }
                if (step is OllamaExtractWorkflowStepSpecifics)
                {
                    route = $"/MyWorkflow/Ollama/{workflowId}/Steps/{step.Id}/UpdateStep";
                }
                if (step is NotionUploadWorkflowStepSpecifics)
                {
                    route = $"/MyWorkflow/Notion/{workflowId}/Steps/{step.Id}/UpdateStep";
                }
                if (step is HelloWorldTextWorkflowStepSpecifics)
                {
                    route = $"/MyWorkflow/Diagnostic/{workflowId}/Steps/{step.Id}/UpdateStep";
                }
                if (step is GcpExtractWorkflowStepSpecifics)
                {
                    route = $"/MyWorkflow/Gcp/{workflowId}/Steps/{step.Id}/UpdateStep";
                }
                if (step is OneDriveUploadWorkflowStepSpecifics)
                {
                    route = $"/MyWorkflow/OneDrive/{workflowId}/Steps/{step.Id}/UpdateStep";
                }

                if (!string.IsNullOrEmpty(route))
                {
                    entityLine += ":::clickable";

                    clicksBuilder
                        .AppendLine(
                            $"click {currentChildAlias} call blazorNavigateToRoute(\"{route}\")"
                        );
                }

                entitiesBuilder
                    .AppendLine(entityLine);

                WriteNested(
                    workflowId,
                    currentChildAlias,
                    entitiesBuilder,
                    linksBuilder,
                    clicksBuilder,
                    styleBuilder,
                    step.ChildSteps,
                    aliasEnumerator,
                    step.OutputTypeName,
                    step.Id
                );
            }

            aliasEnumerator
                .MoveNext();

            if (!string.IsNullOrEmpty(currentParentId))
            {
                // add the "+ Add Step" option for each child - IF it has an output type
                if (currentParentOutputTypeName != DomainConstants.WorkflowFormatTypes[(int)WorkflowFormatTypeEnum.Void])
                {
                    entitiesBuilder
                        .AppendLine($"{aliasEnumerator.Current}([{AddNewStep}]):::clickable");

                    linksBuilder
                        .AppendLine($"{currentParentAlias} --> |{currentParentOutputTypeName}| {aliasEnumerator.Current}");

                    clicksBuilder
                        .AppendLine(
                            $"click {aliasEnumerator.Current} call blazorNavigateToRoute(\"/MyWorkflow/{workflowId}/Steps/{currentParentId}/AddStep\")"
                        );

                    styleBuilder
                        .AppendLine($"style {aliasEnumerator.Current} fill:{AddBackgroundButtonColor}");
                }
            }
        }
    }
}