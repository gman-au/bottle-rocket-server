using System.Collections.Generic;

namespace Rocket.Domain.Dashboard
{
    public class LifecycleSummary
    {
        public IEnumerable<LifecycleTotal> LifecyclesByGroup { get; set; }
    }
}