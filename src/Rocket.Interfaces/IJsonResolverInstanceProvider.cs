using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Rocket.Interfaces
{
    public interface IJsonResolverInstanceProvider
    {
        IJsonTypeInfoResolver GetInstance();

        JsonSerializerOptions GetSerializationOptions();
    }
}