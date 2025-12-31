namespace Rocket.Api.Contracts
{
    public class CreateUserResponse : ApiResponse
    {
        public string Username { get; set; }

        public System.DateTime CreatedAt { get; set; }
    }
}