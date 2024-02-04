using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SubjectDto> Subjects { get; set; } = new();
    }
}
