using DemoLinkedIn.Server.Responses;
using DemoLinkedInApi.DTOs;
using DemoLinkedInApi.Responses;
using System.Security.Claims;

namespace DemoLinkedInApi.Repositories.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse<object>> CreateAsync(RegisterDto user);

        Task<GeneralResponse<LoginResponse>> SignInAsync(LoginDTO user);

        Task<GeneralResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenDTO token);

        Task<GeneralResponse<object>> DeleteAccountAsync(string userId);

        Task<GeneralResponse<object>> EmailConfirmationAsync(string email, int code);

        Task<GeneralResponse<object>> ResendCodeEmailVerificationAsync(string email);

        Task<GeneralResponse<UserInfoResponse>> GetUserAsync(string userId);

        Task<GeneralResponse<object>> EditUserProfile(EditUserProfileDto userDto, string userId);

    }
}
