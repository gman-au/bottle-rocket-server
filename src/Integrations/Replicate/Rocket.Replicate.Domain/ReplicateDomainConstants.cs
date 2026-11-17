using Rocket.Domain.Enum;

namespace Rocket.Replicate.Domain
{
    public static class ReplicateDomainConstants
    {
        public const string ConnectorName = "Replicate API Connector";
        public const string ConnectorCode = "REPLICATE_API";
        
        public const string DataLabToExtractTextWorkflowCode = "REPLICATE_DATALAB_TO_EXTRACT_TEXT";
        public const string DataLabToExtractProjectWorkflowCode = "REPLICATE_DATALAB_TO_EXTRACT_PROJECT";
        public const string DataLabToExtractMarkdownWorkflowCode = "REPLICATE_DATALAB_TO_EXTRACT_MARKDOWN";
        
        public const string DeepSeekOcrExtractTextWorkflowCode = "REPLICATE_DEEP_SEEK_OCR_EXTRACT_TEXT";
        public const string GptRetitleFileWorkflowCode = "REPLICATE_GPT_RETITLE_FILE";

        public const string ReplicateStatusStarting = "starting";
        public const string ReplicateStatusProcessing = "processing";
        public const string ReplicateStatusSucceeded = "succeeded";
        
        public static readonly int[] ReplicateExtractInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ImageData
        ];
        
        public static readonly int[] ReplicateRetitleFileInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.Markdown
        ];

        public const int ReplicateRetitleFileOutputType = (int)WorkflowFormatTypeEnum.RawTextData;
        
        
        public const int ReplicateExtractTextOutputType = (int)WorkflowFormatTypeEnum.RawTextData;
        public const int ReplicateExtractProjectTaskOutputType = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData;
        public const int ReplicateRetitleFileTaskOutputType = (int)WorkflowFormatTypeEnum.RawTextData;
        public const int ReplicateExtractMarkdownOutputType = (int)WorkflowFormatTypeEnum.Markdown;
    }
}