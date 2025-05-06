using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoLinkedIn.Server.Filters;

public class ValidateModelStateFilter: IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = new BadRequestObjectResult(new
            {
                IsSuccessful = false,
                Message = "Validation failed.",
                Errors = errors, 
                StatusCode = StatusCodes.Status400BadRequest
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}