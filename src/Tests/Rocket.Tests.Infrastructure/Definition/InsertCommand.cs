using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Tests.Infrastructure.Definition
{
    public class InsertCommand<T>
    {
        [BsonElement("insert", Order = 0)]
        public string Collection { get; set; } = null!;

        [BsonElement("documents", Order = 1)]
        public List<T> Documents { get; set; } = [];

        [BsonElement("ordered", Order = 2)]
        public bool Ordered { get; set; } = true;
    }
}