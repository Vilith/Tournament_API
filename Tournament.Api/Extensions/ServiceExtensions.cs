using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tournament.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
