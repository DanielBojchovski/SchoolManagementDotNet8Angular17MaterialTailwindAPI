namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Requests
{
    public class UpdateProfessorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SchoolId { get; set; }
    }
}
