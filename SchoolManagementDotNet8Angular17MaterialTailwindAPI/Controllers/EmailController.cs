using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.EmailNotification.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _service;

        public EmailController(IEmailService service)
        {
            _service = service;
        }

        [HttpPost("SendEmail")]
        public async Task<ActionResult<SendEmailResponse>> SendEmail(SendEmailRequest request)
        {
            return await _service.SendEmail(request);
        }
    }
}
