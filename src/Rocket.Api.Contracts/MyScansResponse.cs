using System.Collections.Generic;

namespace Rocket.Api.Contracts
{
    public class MyScansResponse : ApiResponse
    {
        public IEnumerable<MyScanItem> Scans { get; set; }
        
        public int TotalRecords { get; set; }
    }
}