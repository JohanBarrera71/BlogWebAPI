using DemoLinkedIn.Server.Entities;
using DemoLinkedIn.Server.Responses;
using DemoLinkedInApi.Data;
using DemoLinkedInApi.DTOs;
using DemoLinkedInApi.Entities;
using DemoLinkedInApi.Helpers;
using DemoLinkedInApi.Repositories.Contracts;
using DemoLinkedInApi.Responses;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DemoLinkedIn.Server.Repositories.Contracts;

namespace DemoLinkedInApi.Repositories.Implementations
{
    public class UserAccountRepository(
        IEmailService _emailService,
        ApplicationDbContext appDbContext,
        IOptions<JwtSection> config,
        SignInManager<ApplicationUser> siginManager,
        UserManager<ApplicationUser> userManager) : IUserAccount
    {
        public async Task<GeneralResponse<object>> CreateAsync(RegisterDto user)
        {
            if (user is null)
                return new GeneralResponse<object>(false, "Model is empty.", StatusCodes.Status400BadRequest);

            var checkUser = await userManager.FindByEmailAsync(user.Email!);
            if (checkUser is not null)
                return new GeneralResponse<object>(false, "User registered already.", StatusCodes.Status409Conflict);

            // Save user
            ApplicationUser applicationUser = new()
            {
                UserName = user.Email,
                PasswordHash = user.Password,
                Email = user.Email
            };
            IdentityResult result = await userManager.CreateAsync(applicationUser, user.Password!);
            if (!result.Succeeded)
                return new GeneralResponse<object>(false, "User not created.", StatusCodes.Status400BadRequest);

            // Email Confirmation
            var emailCode = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var sendEmail = _emailService.BuildEmail(applicationUser.Email!, emailCode);
            return new GeneralResponse<object>(true, $"Account created! {sendEmail}", StatusCodes.Status201Created);
        }

        public async Task<GeneralResponse<LoginResponse>> SignInAsync(LoginDTO user)
        {
            if (user is null)
                return new GeneralResponse<LoginResponse>(false, "Model is empty.", StatusCodes.Status400BadRequest);

            var applicationUser = await userManager.FindByEmailAsync(user.Email!);
            if (applicationUser is null)
                return new GeneralResponse<LoginResponse>(false, "User not found.", StatusCodes.Status404NotFound);


            // Verify password
            SignInResult checkPassword =
                await siginManager.CheckPasswordSignInAsync(applicationUser, user.Password!, false);
            if (checkPassword.IsNotAllowed)
                return new GeneralResponse<LoginResponse>(false, "Email not confirmed.", StatusCodes.Status403Forbidden,
                    Data: new LoginResponse(EmailConfirmed: false));

            if (!checkPassword.Succeeded)
                return new GeneralResponse<LoginResponse>(false, "Email/Password not valid.",
                    StatusCodes.Status401Unauthorized);

            string jwtToken = GenerateToken(applicationUser);
            string refreshToken = GenerateRefreshToken();

            // Save the refresh token to the database
            var findUser =
                await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.UserId == applicationUser.Id);
            if (findUser is not null)
            {
                findUser!.RefreshToken = refreshToken;
                await appDbContext.SaveChangesAsync();
            }
            else
            {
                await AddToDatabase(new RefreshTokenInfo()
                {
                    Expiration = DateTime.UtcNow.AddDays(1), RefreshToken = refreshToken, UserId = applicationUser.Id
                });
            }

            return new GeneralResponse<LoginResponse>(true, "Login Successfully!",
                Data: new LoginResponse(jwtToken, refreshToken));
        }

        public async Task<GeneralResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenDTO token)
        {
            if (token is null)
                return new GeneralResponse<LoginResponse>(false, "Model is emty.", StatusCodes.Status400BadRequest);

            var findToken =
                await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(
                    _ => _.RefreshToken!.Equals(token.RefreshToken));
            if (findToken is null)
                return new GeneralResponse<LoginResponse>(false, "Refresh token is required.",
                    StatusCodes.Status400BadRequest);

            // Get user details
            var user = await userManager.FindByIdAsync(findToken.UserId);
            if (user is null)
                return new GeneralResponse<LoginResponse>(false,
                    "Refresh token could not be generated because user not found.", StatusCodes.Status404NotFound);

            string jwtToken = GenerateToken(user);
            string refreshToken = GenerateRefreshToken();

            var updateRefreshToken = await appDbContext.RefreshTokenInfos.FirstOrDefaultAsync(_ => _.UserId == user.Id);
            if (updateRefreshToken is null)
                return new GeneralResponse<LoginResponse>(false,
                    "Refresh token could not be generated because user has not signed in",
                    StatusCodes.Status400BadRequest);

            updateRefreshToken.RefreshToken = refreshToken;
            await appDbContext.SaveChangesAsync();
            return new GeneralResponse<LoginResponse>(IsSuccessful: true, Message: "Token refreshed successfully!",
                Data: new LoginResponse(jwtToken, refreshToken));
        }

        public async Task<GeneralResponse<object>> EmailConfirmationAsync(string email, int code)
        {
            if (string.IsNullOrEmpty(email) || code <= 0)
                return new GeneralResponse<object>(false, "Invalid code provided.", StatusCodes.Status400BadRequest);

            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            var result = await userManager.ConfirmEmailAsync(user, code.ToString());
            if (!result.Succeeded)
                return new GeneralResponse<object>(false,
                    "Email could not be confirmed. Invalid code or the code expired.", StatusCodes.Status400BadRequest);

            return new GeneralResponse<object>(true, "Email confirmed successfully.");
        }

        public async Task<GeneralResponse<object>> DeleteAccountAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return new GeneralResponse<object>(false, "UserId is required.", StatusCode: StatusCodes.Status400BadRequest);

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<object>(false, "User not found.", StatusCode:StatusCodes.Status404NotFound);

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return new GeneralResponse<object>(false, "User could not be deleted.", StatusCode:StatusCodes.Status400BadRequest);

            return new GeneralResponse<object>(true, "User deleted successfully.", StatusCode:StatusCodes.Status200OK);
        }

        public async Task<GeneralResponse<UserInfoResponse>> GetUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<UserInfoResponse>(IsSuccessful: false, Message: "User not found.",
                    StatusCodes.Status404NotFound);

            var userInfo = new UserInfoResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Headline = user.Headline,
                ProfilePicture = user.ProfilePicture,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Company = user.Company,
                Position = user.Position,
                Country = user.Country,
                City = user.City,
                Website = user.Website,
                ProfileComplete = user.ProfileComplete
            };

            return new GeneralResponse<UserInfoResponse>
            (
                IsSuccessful: true,
                Message: "User found.",
                Data: userInfo
            );
        }

        public async Task<GeneralResponse<object>> EditUserProfile(EditUserProfileDto userDto, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);
            
             //TODO: Implement this Service and add controller endpoint
            return new GeneralResponse<object>(true, "User updated successfully.");
        }

        public async Task<GeneralResponse<object>> ResendCodeEmailVerificationAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new GeneralResponse<object>(false, "Email is required.", StatusCodes.Status400BadRequest);

            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return new GeneralResponse<object>(false, "User not found.", StatusCodes.Status404NotFound);

            if (await userManager.IsEmailConfirmedAsync(user))
                return new GeneralResponse<object>(false, "Email already confirmed.", StatusCodes.Status409Conflict);

            var emailCode = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var sendEmail = _emailService.BuildEmail(user.Email!, emailCode);
            return new GeneralResponse<object>(true, $"Email verification code resent successfully! {sendEmail}");
        }

        private string GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = appDbContext.Add(model!);
            await appDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }
    }
}