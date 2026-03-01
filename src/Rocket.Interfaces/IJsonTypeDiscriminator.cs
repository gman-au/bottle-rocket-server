using System;

namespace Rocket.Interfaces
{
    public interface IJsonTypeDiscriminator<T>
    {
        public Type Key { get; }
        
        public string Value { get; }
    }
}