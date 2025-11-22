using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EcommerceAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            object resultObj = new
            {
                success = false,
                message = "An error occurred",
                errors = (object?)null,
                statusCode = (int)HttpStatusCode.InternalServerError
            };

            switch (exception)
            {
                case ValidationException ve:
                    code = HttpStatusCode.BadRequest;
                    resultObj = new
                    {
                        success = false,
                        message = "Validation failed",
                        errors = ve.Errors.GroupBy(e => e.PropertyName)
                                        .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray()),
                        statusCode = (int)HttpStatusCode.BadRequest
                    };
                    break;
                case KeyNotFoundException knf:
                    code = HttpStatusCode.NotFound;
                    resultObj = new
                    {
                        success = false,
                        message = knf.Message,
                        errors = (object?)null,
                        statusCode = (int)HttpStatusCode.NotFound
                    };
                    break;
                case UnauthorizedAccessException ua:
                    code = HttpStatusCode.Unauthorized;
                    resultObj = new
                    {
                        success = false,
                        message = ua.Message,
                        errors = (object?)null,
                        statusCode = (int)HttpStatusCode.Unauthorized
                    };
                    break;
                default:
                    resultObj = new
                    {
                        success = false,
                        message = exception.Message,
                        errors = (object?)null,
                        statusCode = (int)HttpStatusCode.InternalServerError
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(resultObj));
        }
    }
}