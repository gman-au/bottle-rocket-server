using System;

namespace Rocket.Api.Contracts
{
    public class UserItem
    {
        public string Id { get; set; }
        
        public string Username { get; set; }
        
        public DateTime? CreatedAt { get; set; }
    }
}