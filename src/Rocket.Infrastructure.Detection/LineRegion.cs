namespace Rocket.Infrastructure.Detection
{
    public class LineRegion
    {
        public int? SymbolIndex { get; set; }
        
        public float Start { get; set; }
        
        public float End { get; set; }
        
        public int DarkPixelCount { get; set; }
    }
}