using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Interfaces;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Consts;

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

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<LoginResponse>> RefreshToken(RefreshTokenRequest request)
        {
            return await _authService.RefreshToken(request);
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult<OperationStatusResponse>> ChangePassword(ChangePasswordRequest request)
        {
            return await _authService.ChangePassword(request);
        }

        [HttpPost("ForgotPasswordSendEmail")]
        public async Task<ActionResult<OperationStatusResponse>> ForgotPasswordSendEmail(ForgotPasswordSendEmailRequest request)
        {
            return await _authService.ForgotPasswordSendEmail(request);
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<OperationStatusResponse>> ResetPassword(ResetPasswordRequest request)
        {
            return await _authService.ResetPassword(request);
        }

        [HttpPost("MakeAdmin")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<OperationStatusResponse> MakeAdmin(UpdatePermissionRequest request)
        {
            return await _authService.MakeAdmin(request);
        }

        [HttpPost("GetAvailableUsers")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<GetAvailableUsersResponse> GetAvailableUsers(GetAvailableUsersRequest request)
        {
            return await _authService.GetAvailableUsers(request);
        }
    }
}
