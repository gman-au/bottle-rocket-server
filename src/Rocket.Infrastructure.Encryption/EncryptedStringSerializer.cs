using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Encryption
{
    public class EncryptedStringSerializer(
        ISecretProtector secretProtector
    ) : SerializerBase<string>
    {
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            string value
        )
        {
            if (value is null)
            {
                context
                    .Writer
                    .WriteNull();
                return;
            }

            context
                .Writer
                .WriteString(
                    secretProtector
                        .Protect(value)
                );
        }

        public override string Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args
        )
        {
            if (context.Reader.CurrentBsonType == BsonType.Null)
            {
                context
                    .Reader
                    .ReadNull();

                return null;
            }

            return
                secretProtector
                    .Unprotect(
                        context
                            .Reader
                            .ReadString()
                    );
        }
    }
}