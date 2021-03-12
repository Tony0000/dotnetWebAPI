using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.ActionResult.Model
{
    public class ValidationResult
    {
        public int StatusCode { get; }
        public string Message { get; }
        public ICollection<ValidationError> Errors { get; set; }

        public ValidationResult(ModelStateDictionary modelState)
        {
            StatusCode = (int) HttpStatusCode.UnprocessableEntity;
            Message = "The model received is invalid and cannot be processed. " +
                      "Check the error list for more details.";
            Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors
                    .Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
        }
    }
}
