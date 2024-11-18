namespace MailService.Model
{
    public class FeedbackModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string Subject
        {
            get
            {
                return $"Request from {Name}";
            }
        }
        public IDictionary<string,string> ToDictionary()
        {
                return new Dictionary<string, string> { { "Name" , Name },
                    { "Email" , Email },
                    { "Phone", Phone },
                    { "Message", Message } };
        }
    }
}
