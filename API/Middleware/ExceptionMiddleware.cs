using FluentValidation;
using Newtonsoft.Json;
using SendGrid.Helpers.Errors.Model;
using System.Net;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string result = JsonConvert.SerializeObject(new ErrorDetails { ErrorMessage = ex.Message, ErrorType = "FAILURE" });

            switch (ex)
            {
                case BadHttpRequestException badHttpRequestException:
                    statusCode = HttpStatusCode.BadRequest; break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound; break;

                case ValidationException validationException:
                    statusCode = HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(validationException.Errors);
                    break;

                default:
                    break;
            }
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }

    public class ErrorDetails
    {
        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }
    }
}