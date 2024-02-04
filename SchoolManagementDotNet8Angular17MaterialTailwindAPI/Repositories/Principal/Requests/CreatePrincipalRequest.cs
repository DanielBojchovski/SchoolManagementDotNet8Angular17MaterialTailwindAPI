namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Requests
{
    public class CreatePrincipalRequest
    {
        public string Name { get; set; } = string.Empty;
        public int SchoolId { get; set; }
    }
}
