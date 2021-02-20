using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ServicesTools
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IBaseRequest
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Handling {TRequest} request: {Request}", typeof(TRequest).ToString(),
                JsonSerializer.Serialize(request));

            TResponse result;
            try
            {
                result = await next();
            }
            catch (Exception e) when (LogError(e))
            {
                // we just re-throw the exception that will be handled in global exc handler
                throw;
            }
            
            // so Unit is internal type coming from MediatR, we dont need to log this
            // but it is used by mediatr for handlers without Response type as a return type
            if (result?.GetType() != typeof(Unit))
            {
                _logger.LogInformation("Response: {Response}", JsonSerializer.Serialize(result));
            }

            return result;
        }
        
        private bool LogError(Exception exception)
        {
            _logger.LogError(exception, string.Empty);
            return true;
        }
    }
}