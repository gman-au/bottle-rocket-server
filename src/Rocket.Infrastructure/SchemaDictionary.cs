using System;
using System.Collections.Generic;
using Rocket.Domain.Enum;
using Rocket.Interfaces;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Infrastructure
{
    public class SchemaDictionary : ISchemaDictionary
    {
        private readonly Dictionary<RocketbookPageTemplateTypeEnum, Type> _schemaTypes = new()
        {
            { RocketbookPageTemplateTypeEnum.NotSet, typeof(string) },
            { RocketbookPageTemplateTypeEnum.StandardLined, typeof(string) },
            { RocketbookPageTemplateTypeEnum.StandardDotted, typeof(string) },
            { RocketbookPageTemplateTypeEnum.ProjectTaskTracker, typeof(ProjectTaskTrackerSchema) }
        };

        public Type From(RocketbookPageTemplateTypeEnum? schemaType)
        {
            schemaType ??= RocketbookPageTemplateTypeEnum.NotSet;

            if (_schemaTypes.TryGetValue(
                    schemaType.Value,
                    out var from
                ))
                return from;

            throw new Exception($"Schema {schemaType} not found in schema dictionary");
        }
    }
}