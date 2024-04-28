using Global.Exception.Handling.Extensions;
using Global.Exception.Handling.Http;
using Global.Exception.Handling.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Global.Exception.Handling.Controllers
{
    public abstract class ApplicationBaseController<T>(ILogger<T> logger) : Controller where T : ApplicationBaseController<T>
    {
        protected readonly ILogger _logger = logger;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _logger.LogDebug("{@logMessageHeader} OnActionExecuting({@context})", Request.LogMessageHeader(), nameof(context));
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            _logger.LogDebug("{@logMessageHeader} OnActionExecuted({@context})", Request.LogMessageHeader(), nameof(context));
        }


        [HttpGet("/Error/{id?}")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int id)
        {
            if (!HttpContext.Response.Headers.ContainsKey("X-Robots-Tag"))
                HttpContext.Response.Headers.Append("X-Robots-Tag", "noindex");

            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            KeyValuePair<int, string> kvp = StatusCodeDefinitions.GetCodeDefinition(id);
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ErrorViewModel viewModel = new()
            {
                RequestId = requestId,
                StatusCode = kvp.Key,
                StatusMessage = kvp.Value,
                Exception = exceptionHandlerPathFeature?.Error
            };

            return View(viewModel);
        }
    }
}
