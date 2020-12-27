using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.ActionResult;

namespace WebAPI.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public virtual ValidationFailedObjectResult ValidationFailed(
            [ActionResultObjectValue] ModelStateDictionary modelState)
        {
            if(modelState == null)
                throw new ArgumentNullException(nameof(ModelState));
            return new ValidationFailedObjectResult(modelState);
        }


    }
}
