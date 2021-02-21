using System;
using FluentValidation;
using MediatR;

namespace Notifications.Features.TodoItemCreatedListener
{
    public class TodoItemCreatedEvent : IRequest
    {
        public Guid TodoListId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class TodoItemCreatedEventValidator : AbstractValidator<TodoItemCreatedEvent>
    {
        public TodoItemCreatedEventValidator()
        {
            RuleFor(e => e.TodoListId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.Name).NotEmpty();
        }
    }
}