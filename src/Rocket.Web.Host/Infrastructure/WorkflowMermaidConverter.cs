using System.Collections.Generic;
using System.Text;
using Rocket.Api.Contracts.Workflows;
using Rocket.Domain.Enum;
using Rocket.Domain.Utils;
using Rocket.Dropbox.Contracts;

namespace Rocket.Web.Host.Infrastructure
{
    public class WorkflowMermaidConverter : IWorkflowMermaidConverter
    {
        private const string StartNode = "Bottle Rocket";
        private const string AddNewStep = "+ Add New Step";
        private const string AddBackgroundButtonColor = "#a8ef6d";

        public string Convert(WorkflowSummary workflow)
        {
            using var aliasEnumerator =
                GenerateSequence()
                    .GetEnumerator();

            var result = new StringBuilder();

            aliasEnumerator.MoveNext();
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
                null,
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

            var mermaid = result.ToString();

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

                var missingConnection = string.IsNullOrEmpty(step.ConnectionId);

                var currentChildAlias =
                    aliasEnumerator
                        .Current;

                var entityLine = $"{currentChildAlias}";
                entityLine += "(\"";
                if (missingConnection)
                {
                    entityLine += "\u26a0\ufe0f";
                }
                else
                {
                    entityLine += "\u2705";
                }
                entityLine += $" {step.StepName}";
                entityLine += "\")";

                linksBuilder
                    .AppendLine($"{currentParentAlias} --> |{step.InputTypeName}| {currentChildAlias}");

                var route = string.Empty;
                if (step is DropboxUploadWorkflowStepSpecifics dropboxUploadStep)
                {
                    route = $"/MyWorkflow/Dropbox/{workflowId}/Steps/{step.Id}/UpdateStep";
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

        private static IEnumerable<string> GenerateSequence(string start = "a")
        {
            var chars = start.ToCharArray();

            while (true)
            {
                yield return new string(chars);

                // Increment the sequence
                var carry = true;
                for (var i = chars.Length - 1; i >= 0 && carry; i--)
                    if (chars[i] < 'z')
                    {
                        chars[i]++;
                        carry = false;
                    }
                    else
                    {
                        chars[i] = 'a';
                    }

                // If we still have a carry, we need an extra character
                if (!carry) continue;
                {
                    chars = new char[chars.Length + 1];
                    for (var i = 0; i < chars.Length; i++)
                        chars[i] = 'a';
                }
            }
        }
    }
}