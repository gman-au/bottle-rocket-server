using System;

namespace Rocket.Interfaces
{
    public interface ITokenExpiryProvider
    {
        TimeSpan GetAccessExpiryLifetime();
        
        TimeSpan GetRefreshExpiryLifetime();
    }
}