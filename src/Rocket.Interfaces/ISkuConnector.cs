namespace Rocket.Interfaces
{
    public interface ISkuConnector
    {
        public string Name { get; }
        
        public string Href { get; }
        
        public string ImagePath { get; }
    }
}