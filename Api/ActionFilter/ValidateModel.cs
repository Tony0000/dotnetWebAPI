using Api.ActionResult;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.ActionFilter
{
    public class ValidateModel : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new ValidationFailedObjectResult(context.ModelState);
        }
    }
}