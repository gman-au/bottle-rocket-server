using System;

namespace Rocket.Interfaces
{
    public interface ISchemaGenerator
    {
        string Generate(Type schemaType);
    }
}