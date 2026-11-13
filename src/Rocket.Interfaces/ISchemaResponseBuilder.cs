using System;

namespace Rocket.Interfaces
{
    public interface ISchemaResponseBuilder
    {
        public object Build(
            string messageContent,
            Type schemaType
        );
    }
}