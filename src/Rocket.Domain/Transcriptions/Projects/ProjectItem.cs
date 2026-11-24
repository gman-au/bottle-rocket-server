namespace Rocket.Domain.Transcriptions.Projects
{
    public record ProjectItem
    {
        public string Project { get; set; }
        
        public string Task { get; set; }
        
        public string DueDate { get; set; }
        
        public string EstimatedTime { get; set; }
    }
}