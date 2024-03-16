using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Responses;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<OperationStatusResponse> Register(RegisterUserRequest request);
        Task<OperationStatusResponse> ConfirmEmail(ConfirmEmailRequest request);
        Task<OperationStatusResponse> ResendEmailConfirmation(ResendEmailConfirmationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<LoginResponse> RefreshToken(RefreshTokenRequest request);
        Task<OperationStatusResponse> ChangePassword(ChangePasswordRequest request);
        Task<OperationStatusResponse> ForgotPasswordSendEmail(ForgotPasswordSendEmailRequest request);
        Task<OperationStatusResponse> ResetPassword(ResetPasswordRequest request);
        Task<OperationStatusResponse> MakeAdmin(UpdatePermissionRequest request);
        Task<GetAvailableUsersResponse> GetAvailableUsers(GetAvailableUsersRequest request);
    }
}
