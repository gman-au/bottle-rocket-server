namespace Rocket.Domain.Transcriptions.WeeklyPlans
{
    public record WeeklyPlanTranscription : BaseTranscription
    {
        public string[] Monday { get; set; }
        
        public string[] Tuesday { get; set; }
        
        public string[] Wednesday { get; set; }
        
        public string[] Thursday { get; set; }
        
        public string[] Friday { get; set; }
        
        public string[] Saturday { get; set; }
        
        public string[] Sunday { get; set; }
    }
}