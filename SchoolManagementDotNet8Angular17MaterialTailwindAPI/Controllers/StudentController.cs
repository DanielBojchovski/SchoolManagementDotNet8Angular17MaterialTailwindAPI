using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Student.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet("GetAllStudents")]
        public async Task<ActionResult<GetAllStudentsResponse>> GetAllStudents()
        {
            return await _service.GetAllStudents();
        }

        [HttpPost("GetStudentById")]
        public async Task<ActionResult<StudentModel?>> GetStudentById(IdRequest request)
        {
            var response = await _service.GetStudentById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("GetStudentWithHisMajor")]
        public async Task<ActionResult<GetStudentWithHisMajorResponse?>> GetStudentWithHisMajor(IdRequest request)
        {
            var response = await _service.GetStudentWithHisMajor(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("SetNewMajorForStudent")]
        public async Task<ActionResult<OperationStatusResponse>> SetNewMajorForStudent(SetNewMajorForStudentRequest request)
        {
            return await _service.SetNewMajorForStudent(request);
        }

        [HttpPost("CreateStudent")]
        public async Task<ActionResult<OperationStatusResponse>> CreateStudent(CreateStudentRequest request)
        {
            return await _service.CreateStudent(request);
        }

        [HttpPost("UpdateStudent")]
        public async Task<ActionResult<OperationStatusResponse>> UpdateStudent(UpdateStudentRequest request)
        {
            return await _service.UpdateStudent(request);
        }

        [HttpPost("DeleteStudent")]
        public async Task<ActionResult<OperationStatusResponse>> DeleteStudent(IdRequest request)
        {
            return await _service.DeleteStudent(request);
        }
    }
}
