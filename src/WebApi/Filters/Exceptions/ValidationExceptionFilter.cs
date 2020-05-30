using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMediaLists.Application.Contracts.Common.Validators;

namespace SocialMediaLists.WebApi.Filters.Exceptions
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var validationException = context.Exception as ValidationException;
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new JsonResult(validationException.ValidationResult);
                context.ExceptionHandled = true;
            }
        }
    }
}