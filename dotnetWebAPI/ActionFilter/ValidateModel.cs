using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.ActionResult;

namespace WebAPI.ActionFilter
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