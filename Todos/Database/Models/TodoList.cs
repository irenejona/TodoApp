using System;
using System.Collections.Generic;

#pragma warning disable 8632
namespace Todos.Database.Models
{
    public class TodoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<TodoItem>? TodoItems { get; set; }
    }
}
#pragma warning restore 8632
