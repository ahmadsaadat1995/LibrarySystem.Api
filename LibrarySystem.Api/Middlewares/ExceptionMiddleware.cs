using LibrarySystem.Api.Errors;
using System.Diagnostics;
using System.Text.Json;

namespace LibrarySystem.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        private ILogger<ExceptionMiddleware> _logger;
        private IHostEnvironment _environment;
        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }
        
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex ,ex.Message);
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";

                var response = _environment.IsDevelopment() ?
                    new ApiExceptionResponse(httpContext.Response.StatusCode,ex.Message , ex.StackTrace?.ToString() ):
                    new ApiExceptionResponse(httpContext.Response.StatusCode ,"An Error Occered , please Try Again Later");
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(response, options);
               await httpContext.Response.WriteAsync(JsonResponse);
            }
            
        }
    }
}
