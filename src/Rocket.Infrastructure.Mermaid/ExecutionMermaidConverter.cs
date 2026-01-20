using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Executions;
using Rocket.Infrastructure.Mermaid.Extensions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mermaid
{
    public class ExecutionMermaidConverter(ILogger<ExecutionMermaidConverter> logger) : IExecutionMermaidConverter
    {
        public string Convert(ExecutionSummary execution)
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
                .AppendLine($"{parent}@{{ shape: lin-rect, label: \"This execution\"}}");

            var executionId =
                execution
                    .Id;

            WriteNested(
                executionId,
                parent,
                entitiesBuilder,
                linksBuilder,
                clicksBuilder,
                styleBuilder,
                execution.Steps,
                aliasEnumerator,
                null,
                null
            );

            aliasEnumerator
                .MoveNext();

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
            IEnumerable<ExecutionStepSummary> steps,
            IEnumerator<string> aliasEnumerator,
            string currentParentOutputTypeName,
            string currentParentId
        )
        {
            foreach (var step in steps ?? [])
            {
                aliasEnumerator
                    .MoveNext();

                var missingConnection = string.IsNullOrEmpty(step.ConnectorId);

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
        }
    }
}