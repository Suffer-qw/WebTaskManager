using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using WebTaskManager.Contracts;

namespace WebTaskManager.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
       
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occued while processing your request");

            var response = new ErrorResponse
            {
                Message = exception.Message,
                Title = exception.GetType().Name,
            };

            switch (exception)
            {
                case BadHttpRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NoTaskFoundException:
                    response.StatusCode= (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response.StatusCode= (int)HttpStatusCode.InternalServerError;
                    break;
            }

            httpContext.Response.StatusCode = response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
