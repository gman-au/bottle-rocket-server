using Rocket.Domain.Enum;

namespace Rocket.Ollama.Domain
{
    public static class OllamaDomainConstants
    {
        public const string ConnectorName = "Ollama Connector";
        public const string ConnectorCode = "OLLAMA_SERVER";
        
        public const string OllamaExtractTextWorkflowCode = "OLLAMA_EXTRACT_TEXT";
        public const string OllamaExtractProjectTasksWorkflowCode = "OLLAMA_EXTRACT_PROJECT";
        public const string OllamaRetitleFileWorkflowCode = "OLLAMA_RETITLE_FILE";
        public const string OllamaExtractWeeklyPlanWorkflowCode = "OLLAMA_EXTRACT_WEEKLY_PLAN";
        
        public static readonly int[] OllamaExtractTextInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int OllamaExtractTextOutputType = (int)WorkflowFormatTypeEnum.RawTextData;
        
        public static readonly int[] OllamaExtractProjectTaskInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int OllamaExtractProjectTaskOutputType = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData;
        
        public static readonly int[] OllamaRetitleFileInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.Markdown,
            (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData,
            (int)WorkflowFormatTypeEnum.WeeklyPlanData
        ];

        public const int OllamaRetitleFileOutputType = (int)WorkflowFormatTypeEnum.Inherited;
        
        public static readonly int[] OllamaExtractWeeklyPlanTaskInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ImageData
        ];
        
        public const int OllamaExtractWeeklyPlanTaskOutputType = (int)WorkflowFormatTypeEnum.WeeklyPlanData;
    }
}