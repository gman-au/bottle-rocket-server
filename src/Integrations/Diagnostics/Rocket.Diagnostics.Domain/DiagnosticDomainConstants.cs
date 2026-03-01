using Rocket.Domain.Enum;

namespace Rocket.Diagnostics.Domain
{
    public static class DiagnosticDomainConstants
    {
        public const string HelloWorldTextWorkflowCode = "HELLO_WORLD_TEXT";
        public const string HelloWorldProjectWorkflowCode = "HELLO_WORLD_PROJECT";
        
        public static readonly int[] HelloWorldTextInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int HelloWorldTextOutputType = (int)WorkflowFormatTypeEnum.RawTextData;
        
        public static readonly int[] HelloWorldProjectInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];
        
        public const int HelloWorldProjectOutputType = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData;
    }
}