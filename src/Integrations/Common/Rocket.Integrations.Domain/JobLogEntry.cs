namespace Rocket.Integrations.Domain
{
    public record JobLogEntry
    {
        public string ScanId { get; set; }
        
        public string LogMessage { get; set; }
    }
}