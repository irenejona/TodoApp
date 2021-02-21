using System.Net;

namespace ServicesTools
{
    public interface IHttpException
    {
        HttpStatusCode ReturnCode { get; }
    }
}