using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<OperationStatusResponse>> Register(RegisterUserRequest request)
        {
            return await _authService.Register(request);    
        }

        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult<OperationStatusResponse>> ConfirmEmail(ConfirmEmailRequest request)
        {
            return await _authService.ConfirmEmail(request);
        }

        [HttpPost("ResendEmailConfirmation")]
        public async Task<ActionResult<OperationStatusResponse>> ResendEmailConfirmation(ResendEmailConfirmationRequest request)
        {
            return await _authService.ResendEmailConfirmation(request);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            return await _authService.Login(request);
        }
    }
}
