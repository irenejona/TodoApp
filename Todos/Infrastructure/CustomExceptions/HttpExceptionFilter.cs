using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Todos.Infrastructure.CustomExceptions
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public HttpExceptionFilter(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        
        public void OnException(ExceptionContext context)
        {
            ProblemDetails errorDetails;
            if (context.Exception is IHttpException httpException)
            {
                errorDetails = new ProblemDetails
                {
                    Title = context.Exception.Message,
                    Status = (int) httpException.ReturnCode,
                    Instance = context.HttpContext.Request.Path,
                    Extensions =
                    {
                        {"exception", _hostEnvironment.IsProduction() ? null : context.Exception?.StackTrace},
                    },
                };
            }
            else if (context.Exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .ToDictionary(e => e.PropertyName, e => new[] {e.ErrorMessage});
                errorDetails = new ValidationProblemDetails(errors)
                {
                    Title = context.Exception.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Instance = context.HttpContext.Request.Path,
                    Extensions =
                    {
                        {"exception", _hostEnvironment.IsProduction() ? null : context.Exception?.StackTrace},
                    },
                };
            }
            else
            {
                errorDetails = new ProblemDetails
                {
                    Title = _hostEnvironment.IsProduction() ? "An internal error occured" : context.Exception?.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.HttpContext.Request.Path,
                    Extensions =
                    {
                        {"exception", _hostEnvironment.IsProduction() ? null : context.Exception?.StackTrace},
                    },
                };
            }
            
            context.Result = new ObjectResult(errorDetails)
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = errorDetails.Status,
            };

            context.ExceptionHandled = true;
        }
    }
}