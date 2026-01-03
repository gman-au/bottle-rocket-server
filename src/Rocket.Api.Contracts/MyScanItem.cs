using System;

namespace Rocket.Api.Contracts
{
    public class MyScanItem
    {
        public string Id { get; set; }
        
        public DateTime DateScanned { get; set; }
        
        public string ContentType { get; set; }
        
        public string ThumbnailBase64 { get; set; }
    }
}