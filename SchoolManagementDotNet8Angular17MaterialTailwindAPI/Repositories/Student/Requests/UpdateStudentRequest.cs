using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Requests
{
    public class UpdateStudentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SubjectInfo> Subjects { get; set; } = new();
    }
}
