using System;

namespace Todos.Features.CreateTodoList
{
    public class TodoListCreatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}