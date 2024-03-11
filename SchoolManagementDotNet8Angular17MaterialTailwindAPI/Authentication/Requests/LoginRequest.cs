using System.ComponentModel.DataAnnotations;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Requests
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
