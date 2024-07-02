using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Theater.Presentation.Pipeline
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class StopwatchActionFilterAttribute : Attribute, IActionFilter
    {
        Stopwatch sw = new Stopwatch();

        public void OnActionExecuted(ActionExecutedContext context)
        {
            sw.Stop();

            context.HttpContext.Response.Headers.TryAdd("Elapsed", $"{sw.ElapsedMilliseconds} ms");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            sw.Start();
        }
    }
}
