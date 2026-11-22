using Rocket.Domain.Enum;

namespace Rocket.OpenAi.Domain
{
    public static class OpenAiDomainConstants
    {
        public const string ConnectorName = "OpenAI Connector";
        public const string ConnectorCode = "OPENAI_CONNECTOR";
        
        public const string OpenAiExtractTextWorkflowCode = "OPENAI_EXTRACT_TEXT";
        public const string OpenAiExtractProjectTasksWorkflowCode = "OPENAI_EXTRACT_PROJECT";
        public const string OpenAiRetitleFileWorkflowCode = "OPENAI_RETITLE_FILE";
        
        public static readonly int[] OpenAiExtractTextInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int OpenAiExtractTextOutputType = (int)WorkflowFormatTypeEnum.RawTextData;
        
        public static readonly int[] OpenAiExtractProjectTaskInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int OpenAiExtractProjectTaskOutputType = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData;
        
        public static readonly int[] OpenAiRetitleFileInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.Markdown
        ];

        public const int OpenAiRetitleFileOutputType = (int)WorkflowFormatTypeEnum.Inherited;
    }
}