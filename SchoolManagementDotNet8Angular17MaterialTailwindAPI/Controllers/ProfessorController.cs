using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Professor.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _service;

        public ProfessorController(IProfessorService service)
        {
            _service = service;
        }

        [HttpGet("GetAllProfessors")]
        public async Task<ActionResult<GetAllProfessorsResponse>> GetAllProfessors()
        {
            return await _service.GetAllProfessors();
        }

        [HttpPost("GetProfessorById")]
        public async Task<ActionResult<ProfessorModel?>> GetProfessorById(IdRequest request)
        {
            var response = await _service.GetProfessorById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("GetProfessorsBySchoolId")]
        public async Task<ActionResult<GetAllProfessorsResponse>> GetProfessorsBySchoolId(IdRequest request)
        {
            return await _service.GetProfessorsBySchoolId(request);
        }

        [HttpPost("CreateProfessor")]
        public async Task<ActionResult<OperationStatusResponse>> CreateProfessor(CreateProfessorRequest request)
        {
            return await _service.CreateProfessor(request);
        }

        [HttpPost("UpdateProfessor")]
        public async Task<ActionResult<OperationStatusResponse>> UpdateProfessor(UpdateProfessorRequest request)
        {
            return await _service.UpdateProfessor(request);
        }

        [HttpPost("DeleteProfessor")]
        public async Task<ActionResult<OperationStatusResponse>> DeleteProfessor(IdRequest request)
        {
            return await _service.DeleteProfessor(request);
        }
    }
}
