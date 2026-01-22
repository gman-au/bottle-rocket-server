using MongoDB.Bson.Serialization.Attributes;

namespace Rocket.Domain.PageTemplates
{
    public class PageTemplate
    {
        [BsonId]
        public string QrCode { get; set; }

        public int PaperSizeType { get; set; }

        public int QrCodeOrientationType { get; set; }

        public int? RocketbookPageTemplateType { get; set; }

        public string SymbolsBoundingBox { get; set; }
    }
}