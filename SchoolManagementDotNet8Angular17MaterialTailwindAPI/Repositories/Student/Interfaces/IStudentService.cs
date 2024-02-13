using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Interfaces
{
    public interface IStudentService
    {
        public Task<GetAllStudentsResponse> GetAllStudents();
        public Task<StudentModel?> GetStudentById(IdRequest request);
        public Task<GetStudentWithHisMajorResponse?> GetStudentWithHisMajor(IdRequest request);
        public Task<SetNewMajorForStudentResponse> SetNewMajorForStudent(SetNewMajorForStudentRequest request);
        public Task<OperationStatusResponse> CreateStudent(CreateStudentRequest request);
        public Task<OperationStatusResponse> UpdateStudent(UpdateStudentRequest request);
        public Task<DeleteStudentResponse> DeleteStudent(IdRequest request);
        public Task<GetAvailableSubjectsResponse> GetAvailableSubjects();
    }
}
