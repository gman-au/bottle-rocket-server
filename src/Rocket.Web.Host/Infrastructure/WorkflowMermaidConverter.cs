using System.Collections.Generic;
using System.Text;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Web.Host.Infrastructure
{
    public class WorkflowMermaidConverter : IWorkflowMermaidConverter
    {
        private const string StartNode = "Bottle Rocket";

        public string Convert(MyWorkflowSummary workflow)
        {
            using var aliasEnumerator =
                GenerateSequence()
                    .GetEnumerator();

            var result = new StringBuilder();

            aliasEnumerator.MoveNext();
            var parent = aliasEnumerator.Current;
            aliasEnumerator.MoveNext();
            var child = aliasEnumerator.Current;

            var entitiesBuilder = new StringBuilder();
            var linksBuilder = new StringBuilder();
            var clicksBuilder = new StringBuilder();

            // start node
            entitiesBuilder
                .AppendLine($"{parent}({StartNode})")
                .AppendLine($"{child}({workflow.Name})");

            linksBuilder
                .AppendLine($"{parent} --> |Image Data| {child}");

            foreach (var step in workflow.Steps)
            {
                WriteNested(
                    child,
                    entitiesBuilder,
                    linksBuilder,
                    clicksBuilder,
                    step,
                    aliasEnumerator
                );
            }

            /*result
                .AppendLine("flowchart TD")
                .AppendLine($"{parent}({workflow.Name}) --> |Image Data| {child}(Is it sunny?)")
                .AppendLine($"B -->|Yes| C[Go outside]")
                .AppendLine($"B -->|No| D[Stay inside]")
                .AppendLine($"C --> E[Have cheese!]")
                .AppendLine($"D --> E");*/

            result
                .AppendLine("flowchart TD")
                .Append(entitiesBuilder)
                .Append(linksBuilder)
                .Append(clicksBuilder);

            var mermaid = result.ToString();

            return
                result
                    .ToString();
        }

        private static void WriteNested(
            string currentParentAlias,
            StringBuilder entitiesBuilder,
            StringBuilder linksBuilder,
            StringBuilder clicksBuilder,
            WorkflowStepSummary currentParent,
            IEnumerator<string> aliasEnumerator
        )
        {
            foreach (var step in currentParent?.ChildSteps ?? [])
            {
                aliasEnumerator.MoveNext();
                var currentChildAlias = aliasEnumerator.Current;

                entitiesBuilder
                    .AppendLine($"{currentChildAlias}({step.StepName}):::clickable");

                linksBuilder
                    .AppendLine($"{currentParentAlias} --> |{step.InputTypeName}| {currentChildAlias}");

                clicksBuilder
                    .AppendLine($"click {currentChildAlias} call blazorNavigate(\"{step.Id}\")");
                
                WriteNested(
                    currentChildAlias,
                    entitiesBuilder,
                    linksBuilder,
                    clicksBuilder,
                    step,
                    aliasEnumerator
                );
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