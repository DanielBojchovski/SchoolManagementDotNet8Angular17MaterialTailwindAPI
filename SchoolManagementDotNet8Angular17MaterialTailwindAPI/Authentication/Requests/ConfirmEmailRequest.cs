namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Requests
{
    public class ConfirmEmailRequest
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
