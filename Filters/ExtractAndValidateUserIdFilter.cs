using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoLinkedIn.Server.Filters;

public class ExtractAndValidateUserIdFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>() != null)
        {
            return;
        }
        
        var user = context.HttpContext.User;

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                IsSuccessful = false,
                Message = "User is not authenticated.",
                StatusCode = StatusCodes.Status401Unauthorized
            });
            return;
        }

        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        {
            context.Result = new BadRequestObjectResult(new
            {
                IsSuccessful = false,
                Message = "User ID is missing in the token.",
                StatusCode = StatusCodes.Status400BadRequest
            });
            return;
        }

        context.HttpContext.Items["UserId"] = userIdClaim.Value;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}