using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Requests;
using SchoolManagementDotNet8Angular17MaterialTailwindAPI.Common.Responses;

namespace SchoolManagementDotNet8Angular17MaterialTailwindAPI.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<OperationStatusResponse> Register(RegisterUserRequest request);
        Task<OperationStatusResponse> ConfirmEmail(ConfirmEmailRequest request);
        Task<OperationStatusResponse> ResendEmailConfirmation(ResendEmailConfirmationRequest request);
    }
}
