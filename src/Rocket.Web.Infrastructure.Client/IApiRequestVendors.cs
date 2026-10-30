using Rocket.Web.Infrastructure.Client.Vendors;

namespace Rocket.Web.Infrastructure.Client
{
    public interface IApiRequestVendors
    {
        IApiRequestDropbox Dropbox { get; }
        
        IApiRequestGoogle Google { get; }
        
        IApiRequestMicrosoft Microsofts { get; }
        
        IApiRequestNotion Notion { get; }
    }
}