using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Rocket.Api.Host.Hubs
{
    [Authorize]
    public class CaptureHub(ILogger<CaptureHub> logger) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = 
                Context
                    .User?
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                    .Value;
            
            if (!string.IsNullOrEmpty(userId))
            {
                // Add user to their own group so we can send targeted messages
                await 
                    Groups
                        .AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                
                logger
                    .LogInformation("User {userId} connected to CaptureHub", userId);
            }
            
            await 
                base
                    .OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = 
                Context
                    .User?
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                    .Value;
            
            if (!string.IsNullOrEmpty(userId))
            {
                logger
                    .LogInformation("User {userId} disconnected from CaptureHub", userId);
            }
            
            await 
                base
                    .OnDisconnectedAsync(exception);
        }
    }
}