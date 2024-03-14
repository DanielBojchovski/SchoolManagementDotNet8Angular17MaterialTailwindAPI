namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Responses
{
    public class LoginResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
