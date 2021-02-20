using System;
using System.Net;

namespace Todos.Infrastructure.CustomExceptions
{
    public class ListNotFound : Exception, IHttpException
    {
        public HttpStatusCode ReturnCode => HttpStatusCode.NotFound;

        public ListNotFound() : base("Todo list not found.")
        {
        }
    }
}