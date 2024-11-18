namespace MailServiceNext.Service
{
    public class CaptchaService
    {
        // Helper method to verify CAPTCHA (Minimal API style)
        public async Task<bool> VerifyCaptchaAsync(string captchaToken, IHttpClientFactory httpClientFactory)
        {
            const string secret = "`<Captcha secure key`>";
            var client = httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={captchaToken}");

            return response.Contains("\"success\": true");
        }
    }
}

