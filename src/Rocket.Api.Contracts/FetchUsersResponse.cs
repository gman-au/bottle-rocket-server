using System.Collections.Generic;

namespace Rocket.Api.Contracts
{
    public class FetchUsersResponse : ApiResponse
    {
        public IEnumerable<UserItem> Users { get; set; }
        
        public int TotalRecords { get; set; }
    }
}