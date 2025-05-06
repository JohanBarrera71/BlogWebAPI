using DemoLinkedIn.Server.Responses;
using DemoLinkedInApi.DTOs;
using DemoLinkedInApi.Repositories.Contracts;
using DemoLinkedInApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoLinkedInApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount accountInterface) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Creates a new user account with the provided registration data.",
            OperationId = "RegisterUser",
            Tags = new[] { "Authentication" }
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "User registered successfully.",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid registration data.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Email already exists.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<ActionResult> CreateAsync(RegisterDto user)
        {
            var result = await accountInterface.CreateAsync(user);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "User login",
            Description = "Authenticates a user and returns a JWT token.",
            OperationId = "LoginUser",
            Tags = new[] { "Authentication" })]
        [SwaggerResponse(StatusCodes.Status200OK, "User logged in successfully.",
            typeof(GeneralResponse<LoginResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid login data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "User email is not confirmed")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> SignInAsync(LoginDTO user)
        {
            var result = await accountInterface.SignInAsync(user);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Refresh token",
            Description = "Generate a new JWT token by comparing the refresh token stored in the database with the one the user has.",
            OperationId = "RefreshToken",
            Tags = new[] { "Authentication" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Token refresh successfully!",
            typeof(GeneralResponse<LoginResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The refresh token does not exist or is invalid || The token does not exist or is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The refresh token may not have been generated because the user does not exist or has not logged in.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO)
        {
            var result = await accountInterface.RefreshTokenAsync(refreshTokenDTO);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("confirm-email/{email}/{code:int}")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Email confirmation",
            Description = "Confirms the user's email address using a confirmation code.",
            OperationId = "ConfirmEmail",
            Tags = new[] { "Authentication" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Email confirmed successfully!",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid email confirmation data.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The email does not exist or the code is invalid.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> EmailConfirmAsync(string email, int code)
        {
            var result = await accountInterface.EmailConfirmationAsync(email, code);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("resend-code-email-verification/{email}")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Resend email verification code",
            Description = "Resends the email verification code to the user's email address.",
            OperationId = "ResendEmailVerificationCode",
            Tags = new[] { "Authentication" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Email confirmed successfully!",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid email confirmation data.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The email does not exist or the code is invalid.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "The email has already been confirmed.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> ResendCodeEmailVerificationAsync(string email)
        {
            var result = await accountInterface.ResendCodeEmailVerificationAsync(email);
            return StatusCode(result.StatusCode, result);
        }

        //TODO: Update profile info

        // TODO: This endpoint should be implemented in another way
        [HttpDelete("delete-account")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UserPolicy")]
        [SwaggerOperation(
            Summary = "Delete accout",
            Description = "Deletes the user's account.",
            OperationId = "DeleteAccount",
            Tags = new[] { "Authentication" })]
        [SwaggerResponse(StatusCodes.Status200OK, "User deleted successfully!",
            typeof(GeneralResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> DeleteAccountAsync()
        {
            var userId = HttpContext.Items["UserId"] as string;

            var result = await accountInterface.DeleteAccountAsync(userId!);
            return Ok(result);
        }

        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UserPolicy")]
        [SwaggerOperation(
            Summary = "Get User",
            Description = "Retrieves the user's information.",
            OperationId = "GetUser",
            Tags = new[] { "User Account" })]
        [SwaggerResponse(StatusCodes.Status200OK, "User found.",
            typeof(GeneralResponse<UserInfoResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found or not exist.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.")]
        public async Task<IActionResult> GetUserAsync()
        {
            var userId = HttpContext.Items["UserId"] as string;

            var result = await accountInterface.GetUserAsync(userId!);
            return StatusCode(result.StatusCode, result);
        }
    }
}