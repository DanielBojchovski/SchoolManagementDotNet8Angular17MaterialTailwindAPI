using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Models;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Repositories.Principal.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        private readonly IPrincipalService _service;

        public PrincipalController(IPrincipalService service)
        {
            _service = service;
        }

        [HttpGet("GetAllPrincipals")]
        public async Task<ActionResult<GetAllPrincipalsResponse>> GetAllPrincipals()
        {
            return await _service.GetAllPrincipals();
        }

        [HttpPost("GetPrincipalById")]
        public async Task<ActionResult<PrincipalModel?>> GetPrincipalById(IdRequest request)
        {
            var response = await _service.GetPrincipalById(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("GetPrincipalBySchoolId")]
        public async Task<ActionResult<PrincipalModel?>> GetPrincipalBySchoolId(IdRequest request)
        {
            var response = await _service.GetPrincipalBySchoolId(request);
            return response == null ? NotFound() : response;
        }

        [HttpPost("CreatePrincipal")]
        public async Task<ActionResult<OperationStatusResponse>> CreatePrincipal(CreatePrincipalRequest request)
        {
            return await _service.CreatePrincipal(request);
        }

        [HttpPost("UpdatePrincipal")]
        public async Task<ActionResult<OperationStatusResponse>> UpdatePrincipal(UpdatePrincipalRequest request)
        {
            return await _service.UpdatePrincipal(request);
        }

        [HttpPost("DeletePrincipal")]
        public async Task<ActionResult<DeletePrincipalResponse>> DeletePrincipal(IdRequest request)
        {
            return await _service.DeletePrincipal(request);
        }

        [HttpGet("GetSchoolsDropDown")]
        public async Task<ActionResult<DropDownResponse>> GetSchoolsDropDown()
        {
            return await _service.GetSchoolsDropDown();
        }

        [HttpPost("InitUpdatePrincipal")]
        public async Task<ActionResult<InitUpdatePrincipalResponse>> InitUpdatePrincipal(IdRequest request)
        {
            return await _service.InitUpdatePrincipal(request);
        }
    }
}
