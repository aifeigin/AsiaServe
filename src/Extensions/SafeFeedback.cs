using MailService.Model;
using MailService.Service;
using MailServiceNext.Model;
using MailServiceNext.Service;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace MailServiceNext.Extensions
{
    public static class SafeFeedback
    {
        public static IResult FeedbackHook(this WebApplication app)
        {
            app.MapPost("/api/feedback", async (FeedbackModel feedback,
                                    [FromHeader(Name = "X-CSRF-TOKEN")] string csrfToken,
                                    [FromHeader(Name = "X-Captcha-Token")] string captchaToken,
                                    IAntiforgery antiforgery,
                                    IHttpClientFactory httpClientFactory,
                                    NativeEmailService emailService,
                                    CaptchaService captchaService,
                                    ILogger<Program> logger,
                                    HttpContext context) =>
            {
                if (feedback == null)
                {
                    return Results.BadRequest("Feedback model is null.");
                }

                logger.LogInformation($"Received Feedback: Name={feedback.Name}, Message={feedback.Message}");

                // CSRF Validation
                try
                {
                    await antiforgery.ValidateRequestAsync(context);
                }
                catch
                {
                    return Results.Json(new VerboseResponse { message = "Invalid CSRF token" }, AppJsonSerializerContext.Default.VerboseResponse, statusCode: StatusCodes.Status401Unauthorized);
                }

                // CAPTCHA Verification
                bool captchaVerified = await captchaService.VerifyCaptchaAsync(captchaToken, httpClientFactory);
                if (!captchaVerified) return Results.BadRequest("CAPTCHA verification failed");

                // Send email feedback
                var result = await emailService.SendEmailAsync(feedback);
                if (!result.success)
                {
                    return Results.Json(new VerboseResponse { message = result.exception != null ? result.exception.Message : "Send failed" }, AppJsonSerializerContext.Default.VerboseResponse, statusCode: StatusCodes.Status500InternalServerError);
                }

                return Results.Ok("Feedback sent successfully!");
            });

            return Results.BadRequest("The request is failed.");
        }
    }
}
