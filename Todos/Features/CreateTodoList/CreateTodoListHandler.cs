using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Todos.Database;
using Todos.Database.Models;
using Todos.Infrastructure;

namespace Todos.Features.CreateTodoList
{
    public class CreateTodoListHandler : AsyncRequestHandler<CreateTodoListCommand>
    {
        private readonly ILogger<CreateTodoListHandler> _logger;
        private readonly TodosDbContext _todosDbContext;
        private readonly IMapper _mapper;
        private readonly IRabbitMqClient _rabbitMqClient;

        public CreateTodoListHandler(ILogger<CreateTodoListHandler> logger,
            TodosDbContext todosDbContext,
            IMapper mapper,
            IRabbitMqClient rabbitMqClient)
        {
            _logger = logger;
            _todosDbContext = todosDbContext;
            _mapper = mapper;
            _rabbitMqClient = rabbitMqClient;
        }
        
        protected override async Task Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var todoList = _mapper.Map<TodoList>(request);
            await _todosDbContext.WrapInTransaction(() =>
                    _todosDbContext.Add(todoList),
                cancellationToken);
            
            _logger.LogInformation("Todo list {TodoListName} added", request.Name);
            
            _logger.LogInformation("Publishing {Event}", Constants.TodoListCreatedRoutingKey);
            _rabbitMqClient.PublishMessage(_mapper.Map<TodoListCreatedEvent>(todoList), Constants.TodoListCreatedRoutingKey);
        }
    }
}