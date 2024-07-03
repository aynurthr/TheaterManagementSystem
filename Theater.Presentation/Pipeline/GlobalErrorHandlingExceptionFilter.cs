using Theater.Infrastructure.Exceptions;
using Theater.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net.Mime;

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
                //case CircleReferenceException crEx:

                //    if (context.HttpContext.Request.IsAjaxRequest())
                //    {
                //        var errors = new Dictionary<string, IEnumerable<string>>
                //        {

                //            [crEx.Property] = new[] { crEx.Message }
                //        };


                //        context.Result = new JsonResult(errors);

                //        return;
                //    }


                //    context.ModelState.AddModelError(crEx.Property, crEx.Message);

                //    var result = new ViewResult()
                //    {
                //        ViewName = context.HttpContext.Request.RouteValues["action"]?.ToString(),
                //        ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                //    };



                //    if (!context.HttpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                //    {
                //        if (context.HttpContext.Request.Form is not null)
                //        {
                //            foreach (var formKey in context.HttpContext.Request.Form.Keys)
                //            {
                //                result.ViewData[formKey] = context.HttpContext.Request.Form[formKey];
                //            }
                //        }
                //    }

                //    context.Result = result;

                //    break;
                case BadRequestException brEx:


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



                    var result2 = new ViewResult()
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
                                result2.ViewData[formKey] = context.HttpContext.Request.Form[formKey];
                            }
                        }
                    }

                    context.Result = result2;

                    break;
                case NotFoundException:
                    if (context.HttpContext.Request.IsAjaxRequest())
                    {
                        context.Result = new NotFoundResult();
                        return;
                    }

                    context.Result = new ContentResult
                    {
                        ContentType = MediaTypeNames.Text.Html, // text/html
                        Content = File.ReadAllText(Path.Combine("wwwroot", "errors", "404.html"))
                    };


                    break;
                default:
                    throw new NotImplementedException();
            }

        }
    }
}
