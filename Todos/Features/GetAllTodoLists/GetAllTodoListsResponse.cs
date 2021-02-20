using System;

namespace Todos.Features.GetAllTodoLists
{
    public class GetAllTodoListsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TodoItemsCount { get; set; }
    }
}