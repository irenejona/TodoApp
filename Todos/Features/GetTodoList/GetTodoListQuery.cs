using System;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Todos.Features.GetTodoList
{
    public class GetTodoListQuery : IRequest<GetTodoListResponse>
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }

    public class GetTodoListQueryValidator : AbstractValidator<GetTodoListQuery>
    {
        public GetTodoListQueryValidator()
        {
            RuleFor(q => q.Id).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}