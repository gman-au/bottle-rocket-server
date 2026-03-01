using Rocket.Domain.Enum;

namespace Rocket.Interfaces
{
    public interface ISchemaResponseBuilder
    {
        public object Build(
            string messageContent,
            RocketbookPageTemplateTypeEnum pageTemplateType
        );
    }
}