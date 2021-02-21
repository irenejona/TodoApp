using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Notifications.Features
{
    public class NotificationsHub : Hub
    {
        private readonly ILogger<NotificationsHub> _logger;

        public NotificationsHub(ILogger<NotificationsHub> logger)
        {
            _logger = logger;
        }
        
        public override async Task OnConnectedAsync()
        {
            var todoListId = Context.GetHttpContext().Request.Query["todoListId"];
            
            _logger.LogInformation("Received connection request to {TodoListId}.Adding to connection group", todoListId);
            await Groups.AddToGroupAsync(Context.ConnectionId, todoListId);
        }
    }
}