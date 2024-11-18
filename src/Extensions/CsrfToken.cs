
namespace MailServiceNext.Extensions
{
    using Microsoft.AspNetCore.Antiforgery;
    using MailService.Model;
    public static class CsrfExt
    {
        public static IResult GetCrfToken(this WebApplication app)
        {
            // CSRF Token endpoint (Minimal API style)
            app.MapGet("/api/get-csrf-token", (IAntiforgery antiforgery, HttpContext context) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                return Results.Ok(new CsrfToken { token = tokens.RequestToken });
            });

            return Results.NoContent();
        }
    }
}
