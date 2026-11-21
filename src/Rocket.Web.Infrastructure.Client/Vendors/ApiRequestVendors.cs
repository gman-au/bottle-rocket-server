namespace Rocket.Web.Infrastructure.Client.Vendors
{
    public class ApiRequestVendors(
        IApiRequestDropbox dropbox,
        IApiRequestGoogle google,
        IApiRequestMicrosoft microsofts,
        IApiRequestNotion notion,
        IApiRequestOllama ollama,
        IApiRequestOpenAi openAi
    )
        : IApiRequestVendors
    {
        public IApiRequestDropbox Dropbox => dropbox;

        public IApiRequestGoogle Google => google;

        public IApiRequestMicrosoft Microsofts => microsofts;

        public IApiRequestNotion Notion => notion;

        public IApiRequestOllama Ollama => ollama;

        public IApiRequestOpenAi OpenAi => openAi;
    }
}