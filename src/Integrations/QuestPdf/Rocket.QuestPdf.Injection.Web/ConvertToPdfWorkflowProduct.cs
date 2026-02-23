using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.QuestPdf.Domain;

namespace Rocket.QuestPdf.Injection.Web
{
    public class ConvertToPdfWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Convert data to PDF file";

        public string Description => "Convert file data (image, text) to a PDF file.";

        public string[] Categories => [SkuConstants.FileConversion];

        public string HrefBase => "/MyWorkflow/ConvertToPdf";

        public string ImagePath => "/img/pdf-logo.png";

        public bool DataLeavesYourServer => false;

        public string StepCode => QuestPdfDomainConstants.ConvertToPdfWorkflowCode;
    }
}