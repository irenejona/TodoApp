using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Database;
using Todos.Database.Models;
using Todos.Infrastructure;
using Todos.Infrastructure.CustomExceptions;

namespace Todos.Features.CreateTodoItem
{
    public class CreateTodoItemHandler : AsyncRequestHandler<CreateTodoItemCommand>
    {
        private readonly ILogger<CreateTodoItemHandler> _logger;
        private readonly TodosDbContext _todosDbContext;
        private readonly IMapper _mapper;
        private readonly IRabbitMqClient _rabbitMqClient;

        public CreateTodoItemHandler(ILogger<CreateTodoItemHandler> logger,
            TodosDbContext todosDbContext,
            IMapper mapper,
            IRabbitMqClient rabbitMqClient)
        {
            _logger = logger;
            _todosDbContext = todosDbContext;
            _mapper = mapper;
            _rabbitMqClient = rabbitMqClient;
        }
        
        protected override async Task Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoList = await _todosDbContext.TodoLists.FindAsync(new object[] { request.TodoListId }, cancellationToken);
            if (todoList is null)
            {
                _logger.LogError("Todo list {TodoListId} not found", request.TodoListId);
                throw new ListNotFound();
            }

            var todoItem = _mapper.Map<TodoItem>(request);
            await _todosDbContext.WrapInTransaction(() =>
                    _todosDbContext.Add(todoItem),
                cancellationToken);
            
            _logger.LogInformation("Todo item added to todo list {TodoListId}", request.TodoListId);
            
            _logger.LogInformation("Publishing {Event}", Constants.TodoItemCreatedRoutingKey);
            _rabbitMqClient.PublishMessage(_mapper.Map<TodoItemCreatedEvent>(todoItem),
                Constants.TodoItemCreatedRoutingKey);
        }
    }
}