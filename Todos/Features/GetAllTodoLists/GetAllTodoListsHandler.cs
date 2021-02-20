using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Todos.Database;

namespace Todos.Features.GetAllTodoLists
{
    public class GetAllTodoListsHandler : IRequestHandler<GetAllTodoListsQuery, IList<GetAllTodoListsResponse>>
    {
        private readonly ILogger<GetAllTodoListsHandler> _logger;
        private readonly TodosDbContext _todosDbContext;
        private readonly IMapper _mapper;

        public GetAllTodoListsHandler(ILogger<GetAllTodoListsHandler> logger,
            TodosDbContext todosDbContext,
            IMapper mapper)
        {
            _logger = logger;
            _todosDbContext = todosDbContext;
            _mapper = mapper;
        }
        
        public async Task<IList<GetAllTodoListsResponse>> Handle(GetAllTodoListsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all TodoLists");

            return await _todosDbContext.TodoListsWithItems()
                .ProjectTo<GetAllTodoListsResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}