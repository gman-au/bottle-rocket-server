using System.Text;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Web.Host.Infrastructure
{
    public class WorkflowMermaidConverter : IWorkflowMermaidConverter
    {
        public string Convert(MyWorkflowSummary workflow)
        {
            var result = new StringBuilder();

            result
                .AppendLine("flowchart TD")
                .AppendLine($"A({workflow.Name}) --> |Image Data| B(Is it sunny?)")
                .AppendLine($"B -->|Yes| C[Go outside]")
                .AppendLine($"B -->|No| D[Stay inside]")
                .AppendLine($"C --> E[Have cheese!]")
                .AppendLine($"D --> E");
            
            return 
                result
                    .ToString();
        }
    }
}