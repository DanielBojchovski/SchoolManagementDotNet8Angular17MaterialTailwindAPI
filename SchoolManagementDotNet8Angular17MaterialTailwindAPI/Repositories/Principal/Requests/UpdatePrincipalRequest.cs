namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Requests
{
    public class UpdatePrincipalRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SchoolId { get; set; }
    }
}
