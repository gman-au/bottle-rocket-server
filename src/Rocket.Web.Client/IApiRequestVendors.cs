using Rocket.Web.Client.Vendors;

namespace Rocket.Web.Client
{
    public interface IApiRequestVendors
    {
        IApiRequestDropbox Dropbox { get; }
        
        IApiRequestGoogle Google { get; }
        
        IApiRequestMicrosoft Microsofts { get; }
        
        IApiRequestNotion Notion { get; }
    }
}