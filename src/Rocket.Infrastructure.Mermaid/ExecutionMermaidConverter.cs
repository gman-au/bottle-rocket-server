using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts.Executions;
using Rocket.Domain.Core.Enum;
using Rocket.Infrastructure.Mermaid.Extensions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mermaid
{
    public class ExecutionMermaidConverter(ILogger<ExecutionMermaidConverter> logger) : IExecutionMermaidConverter
    {
        private const string NotRunFill = "#ccc";
        private const string NotRunStroke = "#333";
        private const string NotRunColor = "#333";
        
        private const string CompletedFill = "#6ee770";
        private const string CompletedStroke = "#185510";
        private const string CompletedColor = "#185510";
        
        private const string ErroredFill = "#e18e8e";
        private const string ErroredStroke = "#891818";
        private const string ErroredColor = "#891818";
        
        private const string CancelledFill = "#e1d28e";
        private const string CancelledStroke = "#894d18";
        private const string CancelledColor = "#894d18";
        
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

            WriteNested(
                parent,
                entitiesBuilder,
                linksBuilder,
                styleBuilder,
                execution.Steps,
                aliasEnumerator
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
            string currentParentAlias,
            StringBuilder entitiesBuilder,
            StringBuilder linksBuilder,
            StringBuilder styleBuilder,
            IEnumerable<ExecutionStepSummary> steps,
            IEnumerator<string> aliasEnumerator
        )
        {
            foreach (var step in steps ?? [])
            {
                aliasEnumerator
                    .MoveNext();

                var currentChildAlias =
                    aliasEnumerator
                        .Current;

                var entityLine = $"{currentChildAlias}";
                entityLine += "(\"";

                switch (step.ExecutionStatus)
                {
                    case (int)ExecutionStatusEnum.Completed:
                        entityLine += "\u2705";
                        styleBuilder
                            .AppendLine($"style {aliasEnumerator.Current} fill:{CompletedFill},stroke:{CompletedStroke},color:{CompletedColor}");
                        break;
                    case (int)ExecutionStatusEnum.Errored:
                        entityLine += "\u2716";
                        styleBuilder
                            .AppendLine($"style {aliasEnumerator.Current} fill:{ErroredFill},stroke:{ErroredStroke},color:{ErroredColor}");
                        break;
                    case (int)ExecutionStatusEnum.NotRun:
                        entityLine += "\ud83d\udec7";
                        styleBuilder
                            .AppendLine($"style {aliasEnumerator.Current} fill:{NotRunFill},stroke:{NotRunStroke},color:{NotRunColor}");
                        break;
                    case (int)ExecutionStatusEnum.Cancelled:
                        entityLine += "\ud83d\udec7";
                        styleBuilder
                            .AppendLine($"style {aliasEnumerator.Current} fill:{CancelledFill},stroke:{CancelledStroke},color:{CancelledColor}");
                        break;
                }

                entityLine += $" {step.StepName}";
                entityLine += "\")";

                linksBuilder
                    .AppendLine($"{currentParentAlias} --> {currentChildAlias}");

                entitiesBuilder
                    .AppendLine(entityLine);

                WriteNested(
                    currentChildAlias,
                    entitiesBuilder,
                    linksBuilder,
                    styleBuilder,
                    step.ChildSteps,
                    aliasEnumerator
                );
            }

            aliasEnumerator
                .MoveNext();
        }
    }
}