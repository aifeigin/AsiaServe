
namespace MailServiceNext.Extensions
{
    using Microsoft.AspNetCore.Antiforgery;
    using MailService.Model;
    using MailServiceNext.Model;

    public static class CsrfExt
    {
        public static IResult GetCrfToken(this WebApplication app)
        {
            // CSRF Token endpoint (Minimal API style)
            app.MapGet("/api/get-csrf-token", (IAntiforgery antiforgery, HttpContext context) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                if (tokens == null)
                {
                    return Results.Json("Csrf token get failed", AppJsonSerializerContext.Default.VerboseResponse, statusCode: StatusCodes.Status500InternalServerError);
                }

                return Results.Ok(new CsrfToken { token = tokens.RequestToken });
            });

            return Results.BadRequest("The request is failed.");
        }
    }
}
