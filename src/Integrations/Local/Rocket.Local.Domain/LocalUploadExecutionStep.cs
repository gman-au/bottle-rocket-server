using Rocket.Domain.Executions;

namespace Rocket.Local.Domain
{
    public record LocalUploadExecutionStep : BaseExecutionStep
    {
        public string UploadPath { get; set; }
    }
}