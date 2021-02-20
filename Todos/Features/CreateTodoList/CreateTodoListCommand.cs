using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Todos.Features.CreateTodoList
{
    public class CreateTodoListCommand : IRequest
    {
        [FromBody]
        public string Name { get; set; }
    }

    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        public CreateTodoListCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}