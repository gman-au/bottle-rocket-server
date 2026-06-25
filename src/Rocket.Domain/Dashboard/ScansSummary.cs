using System.Collections.Generic;

namespace Rocket.Domain.Dashboard
{
    public class ScansSummary
    {
        public int TotalScansReceived { get; set; }
        
        public IEnumerable<ScanByVendorTotal> ScansReceivedByVendor { get; set; }
    }
}