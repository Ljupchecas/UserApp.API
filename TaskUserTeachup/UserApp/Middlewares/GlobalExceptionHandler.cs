using System.ComponentModel.DataAnnotations;

namespace UserApp.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
