using System;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Todos.Features.CreateTodoItem
{
    public class CreateTodoItemCommand : IRequest
    {
        [FromRoute(Name = "id")]
        public Guid TodoListId { get; set; }
        
        [FromBody]
        public AddTodoItemCommandBody Body { get; set; }
    }

#pragma warning disable 8632
    public class AddTodoItemCommandBody
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
#pragma warning restore 8632

    public class AddTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
    {
        public AddTodoItemCommandValidator()
        {
            RuleFor(c => c.TodoListId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(c => c.Body).NotNull()
                .ChildRules(b =>
                {
                    b.RuleFor(n => n.Name).NotEmpty();
                });
        }
    }
}