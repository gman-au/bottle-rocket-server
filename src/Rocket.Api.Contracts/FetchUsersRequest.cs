namespace Rocket.Api.Contracts
{
    public class FetchUsersRequest
    {
        public int StartIndex { get; set; }
        
        public int RecordCount { get; set; }
    }
}