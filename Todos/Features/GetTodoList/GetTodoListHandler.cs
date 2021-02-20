using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Todos.Database;
using Todos.Infrastructure.CustomExceptions;

namespace Todos.Features.GetTodoList
{
    public class GetTodoListHandler : IRequestHandler<GetTodoListQuery, GetTodoListResponse>
    {
        private readonly ILogger<GetTodoListHandler> _logger;
        private readonly TodosDbContext _todosDbContext;
        private readonly IMapper _mapper;

        public GetTodoListHandler(ILogger<GetTodoListHandler> logger,
            TodosDbContext todosDbContext,
            IMapper mapper)
        {
            _logger = logger;
            _todosDbContext = todosDbContext;
            _mapper = mapper;
        }
        
        public async Task<GetTodoListResponse> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Todo list {TodoListId} with items", request.Id);
            var todoList = await _todosDbContext.TodoListsWithItems()
                .ProjectTo<GetTodoListResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(list => list.Id == request.Id, cancellationToken);

            if (todoList is null)
            {
                _logger.LogError("Todo list {TodoListId} not found", request.Id);
                throw new ListNotFound();
            }
            
            _logger.LogInformation("Returning Todo list");
            return todoList;
        }
    }
}