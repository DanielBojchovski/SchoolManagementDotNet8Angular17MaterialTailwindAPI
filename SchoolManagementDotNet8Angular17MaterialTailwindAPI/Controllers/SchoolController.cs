using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.School.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _service;

        public SchoolController(ISchoolService service)
        {
            _service = service;
        }

        [HttpGet("GetAllSchools")]
        public async Task<ActionResult<GetAllSchoolsResponse>> GetAllSchools()
        {
            return await _service.GetAllSchools();
        }

        [HttpPost("GetSchoolById")]
        public async Task<ActionResult<SchoolModel>> GetSchoolById(IdRequest request)
        {
            var response = await _service.GetSchoolById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("GetSchoolByPrincipalId")]
        public async Task<ActionResult<SchoolModel>> GetSchoolByPrincipalId(IdRequest request)
        {
            var response = await _service.GetSchoolByPrincipalId(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("GetSchoolByProfessorId")]
        public async Task<ActionResult<SchoolModel>> GetSchoolByProfessorId(IdRequest request)
        {
            var response = await _service.GetSchoolByProfessorId(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("CreateSchool")]
        public async Task<ActionResult<CreateSchoolResponse>> CreateSchool(CreateSchoolRequest request)
        {
            return await _service.CreateSchool(request);
        }

        [HttpPost("UpdateSchool")]
        public async Task<ActionResult<UpdateSchoolResponse>> UpdateSchool(UpdateSchoolRequest request)
        {
            return await _service.UpdateSchool(request);
        }

        [HttpPost("DeleteSchool")]
        public async Task<ActionResult<DeleteSchoolResponse>> DeleteSchool(IdRequest request)
        {
            return await _service.DeleteSchool(request);
        }
    }
}
