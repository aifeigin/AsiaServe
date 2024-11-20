using Microsoft.AspNetCore.Rewrite;

namespace MailServiceNext.Extensions
{
    public static class RewriteRules
    {
        public static void SetRewriteRules(this WebApplication app)
        {
            // Define redirect rules
            var options = new RewriteOptions()
                // Redirect specific files to their English versions
                .AddRedirect("^qa.html$", "/en/qa.html", StatusCodes.Status301MovedPermanently)
                .AddRedirect("^index.html$", "/en/index.html", StatusCodes.Status301MovedPermanently)
                .AddRedirect("^constellations.html$", "/en/constellations.html", StatusCodes.Status301MovedPermanently)
                .AddRedirect("^offer-qa.html$", "/en/offer-qa.html", StatusCodes.Status301MovedPermanently)
                .AddRedirect("^offer-const.html$", "/en/offer-const.html", StatusCodes.Status301MovedPermanently)
                .AddRedirect("^blog.html$", "/en/blog.html", StatusCodes.Status301MovedPermanently)
                .AddRedirect("^contactform.html$", "/en/contactform.html", StatusCodes.Status301MovedPermanently);

            // Uncomment on publish
#if HAS_HTPPS
if (!app.Environment.IsDevelopment())
{
    options.AddRedirectToHttpsPermanent(); // Redirect HTTP to HTTPS
}
#endif

            // Add the middleware
            app.UseRewriter(options);
        }
    }
}
