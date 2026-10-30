namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestManager
    {
        IApiRequestConnectors Connectors { get; }

        IApiRequestDashboard Dashboard { get; }

        IApiRequestExecutions Executions { get; }
        
        IApiRequestGlobalSettings GlobalSettings { get; }
        
        IApiRequestScans Scans { get; }
        
        IApiRequestSessions Sessions { get; }

        IApiRequestSystem System { get; }
        
        IApiRequestUsers Users { get; }
        
        IApiRequestVendors Vendors { get; }
        
        IApiRequestWorkflows Workflows { get; }
        
        IApiRequestWorkflowSteps WorkflowSteps { get; }
    }
}