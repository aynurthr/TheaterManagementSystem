using Theater.Infrastructure.Exceptions;
using Theater.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net.Mime;

//altered this file

namespace Theater.Presentation.Pipeline
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class GlobalErrorHandlingExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            switch (context.Exception)
            {
                case BadRequestException brEx:
                    HandleBadRequestException(context, brEx);
                    break;
                case NotFoundException:
                    HandleNotFoundException(context);
                    break;
                default:
                    HandleDefaultException(context);
                    break;
            }
        }

        private void HandleBadRequestException(ExceptionContext context, BadRequestException brEx)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult(brEx.Errors);
                return;
            }

            foreach (var property in brEx.Errors)
            {
                foreach (var message in property.Value)
                {
                    context.ModelState.AddModelError(property.Key, message);
                }
            }

            var result = new ViewResult()
            {
                ViewName = context.HttpContext.Request.RouteValues["action"]?.ToString(),
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            };

            if (!context.HttpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                if (context.HttpContext.Request.Form is not null)
                {
                    foreach (var formKey in context.HttpContext.Request.Form.Keys)
                    {
                        result.ViewData[formKey] = context.HttpContext.Request.Form[formKey];
                    }
                }
            }

            context.Result = result;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new NotFoundResult();
                return;
            }

            context.Result = new ContentResult
            {
                ContentType = MediaTypeNames.Text.Html,
                Content = File.ReadAllText(Path.Combine("wwwroot", "errors", "404.html"))
            };
        }

        private void HandleDefaultException(ExceptionContext context)
        {
            var exception = context.Exception;

            var result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<Exception>(new EmptyModelMetadataProvider(), context.ModelState)
                {
                    Model = exception
                }
            };

            context.Result = result;
        }
    }
}
