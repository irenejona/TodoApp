using System;

#pragma warning disable 8632
namespace Todos.Database.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public Guid TodoListId { get; set; }
        public TodoList TodoList { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
#pragma warning restore 8632
