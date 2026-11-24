using System;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public abstract class TranscriptionModelMapperBase<TDomain, TView>
        : ITranscriptionModelMapper<TDomain, TView>
        where TDomain : BaseTranscription, new()
        where TView : TranscriptionSummary, new()
    {
        public bool AppliesFor(Type type) => type == typeof(TView);
        public bool AppliesFrom(Type type) => type == typeof(TDomain);

        public virtual TDomain For(TView value)
        {
            return new TDomain
            {
                Id = value.Id,
                ExecutionId = value.ExecutionId,
                ExecutionStepName = value.ExecutionStepName,
                TranscriptionDate = value.TranscriptionDate
            };
        }

        public virtual TView From(TDomain value)
        {
            return new TView
            {
                Id = value.Id,
                ExecutionId = value.ExecutionId,
                ExecutionStepName = value.ExecutionStepName,
                TranscriptionDate = value.TranscriptionDate
            };
        }

        public Type DomainType => typeof(TDomain);
        public Type ViewType => typeof(TView);

        public BaseTranscription For(object value) => For((TView)value);
        public TranscriptionSummary From(object value) => From((TDomain)value);
    }
}