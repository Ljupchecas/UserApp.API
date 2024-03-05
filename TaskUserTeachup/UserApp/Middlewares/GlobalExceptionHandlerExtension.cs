using UserApp.Middlewares;

namespace UserApp.Middlewares
{
    public static class GlobalExceptionHandlerExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
            return app;
        }
    }
}
