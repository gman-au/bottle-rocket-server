using System;

namespace Rocket.Interfaces
{
    public interface ITranscriptionModelMapperRegistry
    {
        ITranscriptionModelMapper GetMapperForView(Type type);

        ITranscriptionModelMapper GetMapperForDomain(Type type);
    }
}