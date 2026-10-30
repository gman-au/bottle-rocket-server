namespace Rocket.Web.Infrastructure.Client
{
    public class ApiRequestManager(
        IApiRequestConnectors connectors,
        IApiRequestDashboard dashboard,
        IApiRequestExecutions executions,
        IApiRequestGlobalSettings globalSettings,
        IApiRequestScans scans,
        IApiRequestSessions sessions,
        IApiRequestSystem system,
        IApiRequestUsers users,
        IApiRequestVendors vendors,
        IApiRequestWorkflows workflows,
        IApiRequestWorkflowSteps workflowSteps
    ) : IApiRequestManager
    {
        public IApiRequestConnectors Connectors => connectors;
        public IApiRequestDashboard Dashboard => dashboard;
        public IApiRequestExecutions Executions => executions;
        public IApiRequestGlobalSettings GlobalSettings => globalSettings;
        public IApiRequestScans Scans => scans;
        public IApiRequestSessions Sessions => sessions;
        public IApiRequestSystem System => system;
        public IApiRequestUsers Users => users;
        public IApiRequestVendors Vendors => vendors;
        public IApiRequestWorkflows Workflows => workflows;
        public IApiRequestWorkflowSteps WorkflowSteps => workflowSteps;
    }
}