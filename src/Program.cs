using MailService.Extensions;
using MailService.Model;
using MailService.Service;
using MailServiceNext.Extensions;
using MailServiceNext.Model;
using MailServiceNext.Service;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddScoped<NativeEmailService>();

builder.Services.AddScoped<CaptchaService>();

#if DEBUG
string url = "http://localhost:5294, http://localhost";
#else
string url = builder.Configuration.GetSection("SiteSettings").GetValue<string>("Url");
#endif

string defaultPage = builder.Configuration.GetSection("SiteSettings").GetValue<string>("DefaultPage");

string url404 = builder.Configuration.GetSection("SiteSettings").GetValue<string>("404Page");

// Add services for CORS, Anti-forgery, and controllers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(url)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // Set the header name for the anti-forgery token
});

// Register IHttpClientFactory
builder.Services.AddHttpClient();

#if DEBUG
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif

//string encryptedPassword = EncryptionHelper.Encrypt("`<secret mail key`>");
//Console.WriteLine("Encrypted Password: " + encryptedPassword);

var app = builder.Build();

// CSRF Token endpoint (Minimal API style)
app.GetCrfToken();

// Feedback endpoint (Minimal API style)
app.FeedbackHook();

app.UseStatusCodePagesWithReExecute(url404 ?? "/pt/404.html");

// Enable serving static files from wwwroot
app.UseStaticFiles();

app.GetSitemap();

// Enable CORS middleware for the Web API
app.UseCors("AllowSpecificOrigin");

#if DEBUG
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
#endif

// Define redirect rules
app.SetRewriteRules();

// Serve index.html for the root URL
app.MapFallbackToFile(defaultPage ?? "/en/constellations.html");

app.Run();


[JsonSerializable(typeof(FeedbackModel))]
[JsonSerializable(typeof(FeedbackModel[]))]
[JsonSerializable(typeof(CsrfToken))]
[JsonSerializable(typeof(CsrfToken[]))]
[JsonSerializable(typeof(VerboseResponse))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

