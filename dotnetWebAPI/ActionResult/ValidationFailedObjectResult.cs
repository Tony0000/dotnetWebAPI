using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.ActionResult.Model;

namespace WebAPI.ActionResult
{
    public class ValidationFailedObjectResult : ObjectResult
    {
        public ValidationFailedObjectResult(ModelStateDictionary modelState) 
            : base(new ValidationResult(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
