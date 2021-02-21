using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Notifications.Features.TodoItemCreatedListener
{
    public class TodoItemCreatedHandler : AsyncRequestHandler<TodoItemCreatedEvent>
    {
        private readonly ILogger<TodoItemCreatedHandler> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public TodoItemCreatedHandler(ILogger<TodoItemCreatedHandler> logger,
            IHubContext<NotificationsHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }
        
        protected override async Task Handle(TodoItemCreatedEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received todoItemCreatedEvent for TodList {TodoListId}. Notifying todoList group",
                request.TodoListId);
            
            await _hubContext.Clients.Group(request.TodoListId.ToString())
                .SendAsync("notify", JsonSerializer.Serialize(request), cancellationToken);
        }
    }
}