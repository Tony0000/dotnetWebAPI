using System;
using Api.ActionResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        protected ValidationFailedObjectResult ValidationFailed(
            [ActionResultObjectValue] ModelStateDictionary modelState)
        {
            if(modelState == null)
                throw new ArgumentNullException(nameof(ModelState));
            return new ValidationFailedObjectResult(modelState);
        }

        [NonAction]
        protected BadRequestObjectResult BadRequest(
            [ActionResultObjectValue] Exception e)
        {
            return new BadRequestObjectResult(new
            {
                type = e.GetType(), 
                message = e.Message
            });
        }
    }
}
