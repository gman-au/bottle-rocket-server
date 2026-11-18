using Rocket.Domain.Enum;

namespace Rocket.Markdig.Domain
{
    public static class MarkdigDomainConstants
    {
        public const string ConvertToEpubWorkflowCode = "MARKDIG_CONVERT_EPUB";
        
        public static readonly int[] ConvertToEpubInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.Markdown
        ];

        public const int ConvertToEpubOutputType = (int)WorkflowFormatTypeEnum.File;
    }
}