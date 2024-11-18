namespace MailService.Extensions
{
    using System.Text;
    using System.Xml.Linq;
    public static class Sitemap
    {
        // Filter files to exclude certain extensions or directories
        static string[] allowedExtensions = new[] { ".html", ".htm" };
        public static IResult GetSitemap(this WebApplication app)
        {
            // Define the sitemap.xml endpoint using MapGet
            app.MapGet("/sitemap.xml", (IWebHostEnvironment environment, HttpContext context) =>
            {
                var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
                var wwwrootPath = environment.WebRootPath;

                // Recursively find all files in wwwroot
                var files = Directory.GetFiles(wwwrootPath, "*.*", SearchOption.AllDirectories);

                // Map files to relative URLs
                var urls = files
                    .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                    .Select(file =>
                {
                    var relativePath = file.Replace(wwwrootPath, "").Replace("\\", "/").TrimStart('/');
                    return $"{baseUrl}/{relativePath}";
                });

                // Define the XML namespace for the sitemap
                XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

                // Build XML Sitemap
                var sitemap = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("urlset",
                        // new XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                        urls.Select(url =>
                            new XElement(ns + "url",
                                new XElement(ns + "loc", url)
                                //,new XElement("lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd")), // Optional: Use file metadata for last modified
                                //new XElement("changefreq", "monthly"),                        // Optional: Customize change frequency
                                //new XElement("priority", "0.5")                              // Optional: Customize priority
                            )
                        )
                    )
                );

                var xml = sitemap.ToString();
                return Results.Content(xml, "application/xml", Encoding.UTF8);
            });

            return Results.NoContent();
        }
    }
}