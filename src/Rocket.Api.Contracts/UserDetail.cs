using System;

namespace Rocket.Api.Contracts
{
    public class UserDetail : ApiResponse
    {
        public string Id { get; set; }
        
        public string Username { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool? IsActive { get; set; }
        
        public bool? IsAdmin { get; set; }
        
        public string NewPassword { get; set; }
    }
}