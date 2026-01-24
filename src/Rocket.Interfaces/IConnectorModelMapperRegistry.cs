using System;

namespace Rocket.Interfaces
{
    public interface IConnectorModelMapperRegistry
    {
        IConnectorModelMapper GetMapperForView(Type type);

        IConnectorModelMapper GetMapperForDomain(Type type);
    }
}