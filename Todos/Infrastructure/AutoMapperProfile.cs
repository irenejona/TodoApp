using AutoMapper;
using Todos.Database.Models;
using Todos.Features.CreateTodoItem;
using Todos.Features.CreateTodoList;
using Todos.Features.GetAllTodoLists;
using Todos.Features.GetTodoList;

namespace Todos.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTodoItemCommand, TodoItem>()
                .ForMember(item => item.Name, command => command.MapFrom(src => src.Body.Name))
                .ForMember(item => item.Description, command => command.MapFrom(src => src.Body.Description));

            CreateMap<CreateTodoListCommand, TodoList>();

            CreateMap<TodoList, GetAllTodoListsResponse>()
                .ForMember(response => response.TodoItemsCount, list => list.MapFrom(src => src.TodoItems.Count));

            CreateMap<TodoList, GetTodoListResponse>();

            CreateMap<TodoList, TodoListCreatedEvent>();

            CreateMap<TodoItem, TodoItemCreatedEvent>();
        }
    }
}