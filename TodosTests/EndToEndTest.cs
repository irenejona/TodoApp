using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Todos.Features.CreateTodoList;
using Todos.Features.GetAllTodoLists;
using Xunit;

namespace TodosTests
{
    public class EndToEndTest : IClassFixture<WebApplicationFactory<Todos.Startup>>
    {
        private readonly HttpClient _client;

        public EndToEndTest(WebApplicationFactory<Todos.Startup> fixture)
        {
            _client = fixture.CreateClient();
        }
        
        [Fact]
        public async Task Should_GetAllTodoLists()
        {
            // act
            var response = await _client.GetAsync("/todos");
            
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var deserialized = JsonSerializer.Deserialize<GetAllTodoListsResponse[]>(await response.Content.ReadAsStringAsync());
            deserialized.Should().NotBeNull();
        }
        
        
        [Fact]
        public async Task Should_CreateATodoList()
        {
            // act
            var response = await _client.PostAsync("/todos", new StringContent(
                JsonSerializer.Serialize(new CreateTodoListCommand
                {
                    Name = "Some Name"
                }),
                Encoding.UTF8,
                "application/json"));
            
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}