namespace Rocket.Domain.Dashboard
{
    public class DashboardSnapshot
    {
        public ScansSummary Scans { get; set; }
        
        public StorageSummary Storage { get; set; }
        
        public ExecutionsSummary Executions { get; set; }
    }
}