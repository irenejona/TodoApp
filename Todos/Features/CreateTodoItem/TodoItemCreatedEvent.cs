using System;

#pragma warning disable 8632
namespace Todos.Features.CreateTodoItem
{
    public class TodoItemCreatedEvent
    {
        public Guid TodoListId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
#pragma warning restore 8632