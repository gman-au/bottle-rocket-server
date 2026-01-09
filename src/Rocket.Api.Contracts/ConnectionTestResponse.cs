namespace Rocket.Api.Contracts
{
    public class ConnectionTestResponse : ApiResponse
    {
        public string UserName { get; set; }
        
        public string Role { get; set; }
    }
}