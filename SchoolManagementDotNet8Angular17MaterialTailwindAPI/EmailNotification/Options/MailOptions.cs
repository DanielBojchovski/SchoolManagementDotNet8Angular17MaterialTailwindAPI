namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Options
{
    public class MailOptions
    {
        public string Url { get; set; } = string.Empty;
        public string EmailHost { get; set; } = string.Empty;
        public string EmailUserName { get; set; } = string.Empty;
        public string EmailPassword { get; set; } = string.Empty;
        public int Port { get; set; }
        public int SSL { get; set; }
    }
}
