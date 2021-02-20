using System.Collections.Generic;
using MediatR;

namespace Todos.Features.GetAllTodoLists
{
    public class GetAllTodoListsQuery : IRequest<IList<GetAllTodoListsResponse>>
    {
    }
}