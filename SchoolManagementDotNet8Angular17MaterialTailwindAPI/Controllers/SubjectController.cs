using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Subject.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service)
        {
            _service = service;
        }

        [HttpGet("GetAllSubjects")]
        public async Task<ActionResult<GetAllSubjectsResponse>> GetAllSubjects()
        {
            return await _service.GetAllSubjects();
        }

        [HttpPost("GetSubjectById")]
        public async Task<ActionResult<SubjectModel?>> GetSubjectById(IdRequest request)
        {
            var response = await _service.GetSubjectById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("CreateSubject")]
        public async Task<ActionResult<CreateSubjectResponse>> CreateSubject(CreateSubjectRequest request)
        {
            return await _service.CreateSubject(request);
        }

        [HttpPost("UpdateSubject")]
        public async Task<ActionResult<UpdateSubjectResponse>> UpdateSubject(UpdateSubjectRequest request)
        {
            return await _service.UpdateSubject(request);
        }

        [HttpPost("DeleteSubject")]
        public async Task<ActionResult<DeleteSubjectResponse>> DeleteSubject(IdRequest request)
        {
            return await _service.DeleteSubject(request);
        }
    }
}
