using System;

namespace Rocket.Markdig.Infrastructure.Definition
{
    public class Metadata
    {
        public string BookId { get; } = $"urn:uuid:{Guid.NewGuid()}";

        public string Author { get; set; }

        public string Title { get; set; }
    }
}