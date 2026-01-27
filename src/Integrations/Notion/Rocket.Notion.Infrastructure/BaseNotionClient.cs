using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Rocket.Notion.Infrastructure
{
    public class BaseNotionClient
    {
        private const string NotionEndpoint = "https://api.notion.com";
        private const string NotionVersion = "2022-06-28";
        
        protected HttpClient GetBaseHttpClient(string integrationSecret)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress =
                new Uri(NotionEndpoint);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    integrationSecret
                );

            httpClient.DefaultRequestHeaders.Add("Notion-Version", NotionVersion);

            return httpClient;
        } 
    }
}