using System;
using System.Collections.Generic;
using Todos.Database.Models;

namespace Todos.Features.GetTodoList
{
    public class GetTodoListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}