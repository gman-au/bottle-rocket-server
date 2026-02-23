namespace Rocket.Interfaces
{
    public interface ISkuWorkflow
    {
        public string Name { get; }

        public string Description { get; }

        public string[] Categories { get; }

        public string HrefBase { get; }

        public string ImagePath { get; }

        public bool DataLeavesYourServer { get; }

        public string StepCode { get; }
    }
}