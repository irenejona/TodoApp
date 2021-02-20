using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todos.Features.CreateTodoItem;
using Todos.Features.CreateTodoList;
using Todos.Features.GetAllTodoLists;
using Todos.Features.GetTodoList;

namespace Todos.Features
{
    [ApiController, Route("[controller]"), Produces(MediaTypeNames.Application.Json)]
    public class TodosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodos([FromQuery] GetAllTodoListsQuery getAllTodoListsQuery)
        {
            var result = await _mediator.Send(getAllTodoListsQuery);

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] CreateTodoListCommand createTodoListCommand)
        {
            await _mediator.Send(createTodoListCommand);

            return Ok();
        }
                
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo([FromQuery] GetTodoListQuery getTodoListQuery)
        {
            var result = await _mediator.Send(getTodoListQuery);

            return Ok(result);
        }
        
        [HttpPost("{id}")]
        public async Task<IActionResult> AddTodoItem([FromRoute] CreateTodoItemCommand createTodoItemCommand)
        {
            await _mediator.Send(createTodoItemCommand);

            return Ok();
        }
    }
}