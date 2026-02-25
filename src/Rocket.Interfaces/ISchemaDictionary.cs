using System;
using Rocket.Domain.Enum;

namespace Rocket.Interfaces
{
    public interface ISchemaDictionary
    {
        Type From(RocketbookPageTemplateTypeEnum? schemaType);
    }
}