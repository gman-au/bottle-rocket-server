using Rocket.Domain.Enum;

namespace Rocket.QuestPdf.Domain
{
    public static class QuestPdfDomainConstants
    {
        public const string ConvertToPdfWorkflowCode = "QUEST_CONVERT_PDF";
        
        public static readonly int[] ConvertToPdfInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int ConvertToPdfOutputType = (int)WorkflowFormatTypeEnum.File;
    }
}