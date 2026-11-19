using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rocket.Diagnostics.Domain;
using Rocket.Domain.Executions;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldMarkdownHook(
        ILogger<HelloWorldMarkdownHook> logger
    ) : HookBase<HelloWorldMarkdownExecutionStep>(logger), IIntegrationHook
    {
        public async Task<ExecutionStepArtifact> ProcessAsync(
            IWorkflowExecutionContext context,
            BaseExecutionStep step,
            string userId,
            IJobControl jobControl,
            CancellationToken cancellationToken
        )
        {
            context
                .InitializeStep(
                    this,
                    step
                )
                .InitializeArtifact(this);
            
            var sb = new StringBuilder();

            sb
                .AppendLine("# HELLO WORLD")
                .AppendLine()
                .AppendLine("This is formatted markdown.")
                .AppendLine("It can encapsulate **advanced formatting** of text.")
                .AppendLine();

            sb
                .AppendLine("`Inline code is supported`.")
                .AppendLine()
                .AppendLine("```")
                .AppendLine("function HelloWorld() {")
                .AppendLine("    return 'Hello, world!';")
                .AppendLine("}")
                .AppendLine("```")
                .AppendLine();

            sb
                .AppendLine("| Column One | Column Two | Column Three|")
                .AppendLine("|------------|------------|-------------|")
                .AppendLine("| Value One  | Value Two  | Value Three |")
                .AppendLine("| Value Four  | Value Five  | Value Six |")
                .AppendLine();


            var result = new List<(string, string)> { (null, sb.ToString()) };

            return
                result
                    .AsCompletedMarkdownArtifact(Artifact);
        }
    }
}