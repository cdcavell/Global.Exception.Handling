using Global.Exception.Handling.Exceptions;
using Global.Exception.Handling.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Global.Exception.Handling.ExceptionHandlers
{
    internal sealed class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<BadRequestExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BadRequestException badRequestException)
                return false;

            _logger.LogWarning(exception, "{@logMessageHeader} Exception Occurred: {@message}", httpContext.Request.LogMessageHeader(), exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = badRequestException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            if (httpContext.Request.IsAjaxRequest())
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            else
            {
                httpContext.Response.Redirect($"/Error/{problemDetails.Status.Value}");
                await httpContext.Response.StartAsync().ConfigureAwait(false);
            }

            return true;
        }
    }
}
