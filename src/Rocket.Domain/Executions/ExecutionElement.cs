namespace Rocket.Domain.Executions
{
    public record ExecutionElement
    {
        public string Id { get; set; }
        
        public string ScanId { get; set; }
        
        public int ExecutionStatus { get; set; }
        
        public string[] LogMessages { get; set; }
        
        public string ThumbnailBase64 { get; set; }
        
        public string ContentType { get; set; }
        
        public string Vendor { get; set; }
    }
}