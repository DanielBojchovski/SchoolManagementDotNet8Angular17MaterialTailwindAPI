using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Interfaces
{
    public interface IEmailService
    {
        public Task<SendEmailResponse> SendEmail(SendEmailRequest request);
    }
}
