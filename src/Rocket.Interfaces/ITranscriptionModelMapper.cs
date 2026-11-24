using System;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions;

namespace Rocket.Interfaces
{
    public interface ITranscriptionModelMapper
    {
        Type DomainType { get; }
        Type ViewType { get; }
        BaseTranscription For(object value);
        TranscriptionSummary From(object value);
    }
    
    public interface ITranscriptionModelMapper<TDomain, TView> 
        : ITranscriptionModelMapper
        where TDomain : BaseTranscription, new()
        where TView : TranscriptionSummary, new()
    {
        bool AppliesFor(Type type);
        
        bool AppliesFrom(Type type);
        
        TDomain For(TView value);
        
        TView From(TDomain value);
    }
}