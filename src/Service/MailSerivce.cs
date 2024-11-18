namespace MailService.Service
{
    using MailKit.Net.Smtp;
    using MailService.Model;
    using MailServiceNext.Service;
    using MimeKit;
    public class NativeEmailService
    {
        private readonly IConfiguration _configuration;
        private TemplateService? _template;
        public NativeEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<(bool success, Exception? exception)> SendEmailAsync(FeedbackModel feedback)
        {
            try
            {
                var message = new MimeMessage();
                string fromAddress = _configuration["EmailSettings:FromEmail"];                
                message.From.Add(new MailboxAddress(GetEmailUserName(fromAddress), fromAddress));
                string toAddress = _configuration["EmailSettings:ToEmail"];
                message.To.Add(new MailboxAddress(GetEmailUserName(toAddress), toAddress));
                message.ReplyTo.Add(new MailboxAddress(GetEmailUserName(feedback.Email),feedback.Email));                
                message.Subject = feedback.Subject;
                
                if (_template == null)
                {
                    _template = TemplateService.Create();
                }

                var builder = new BodyBuilder { HtmlBody = _template.GetHtmlBody(feedback.ToDictionary()) };
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"]), true);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_configuration["EmailSettings:Username"], EncryptionHelper.Decrypt(_configuration["EmailSettings:Password"]));

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }
        public static NativeEmailService Create()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false,
                reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            return new NativeEmailService(config);
        }

        private string GetEmailUserName(string emaiAddress)
        {
            return new System.Net.Mail.MailAddress(emaiAddress).User;
        }
    }
}
