using System.Net;

namespace Todos.Infrastructure.CustomExceptions
{
    public interface IHttpException
    {
        HttpStatusCode ReturnCode { get; }
    }
}