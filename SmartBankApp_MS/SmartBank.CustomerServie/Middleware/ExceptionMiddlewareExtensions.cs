namespace SmartBank.CustomerServie.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder
            UseCustomExceptionMiddleware(
                this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}