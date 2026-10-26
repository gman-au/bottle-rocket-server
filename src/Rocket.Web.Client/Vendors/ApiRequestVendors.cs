namespace Rocket.Web.Client.Vendors
{
    public class ApiRequestVendors(
        IApiRequestDropbox dropbox,
        IApiRequestGoogle google,
        IApiRequestMicrosoft microsofts,
        IApiRequestNotion notion
    )
        : IApiRequestVendors
    {
        public IApiRequestDropbox Dropbox => dropbox;

        public IApiRequestGoogle Google => google;

        public IApiRequestMicrosoft Microsofts => microsofts;

        public IApiRequestNotion Notion => notion;
    }
}