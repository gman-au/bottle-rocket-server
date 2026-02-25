namespace Rocket.Replicate.Domain
{
    public static class ReplicateDomainConstants
    {
        public const string ConnectorName = "Replicate API Connector";
        public const string ConnectorCode = "REPLICATE_API";
        
        public const string DataLabToExtractTextWorkflowCode = "REPLICATE_DATALAB_TO_EXTRACT_TEXT";
        public const string DataLabToExtractProjectWorkflowCode = "REPLICATE_DATALAB_TO_EXTRACT_PROJECT";
        
        public const string DeepSeekOcrExtractTextWorkflowCode = "REPLICATE_DEEP_SEEK_OCR_EXTRACT_TEXT";

        public const string ReplicateStatusStarting = "starting";
        public const string ReplicateStatusProcessing = "processing";
        public const string ReplicateStatusSucceeded = "succeeded";
    }
}