using System.Linq;
using Data;
using Domain.Model.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.ActionFilters
{
    public class NotFoundAttribute<T> : IActionFilter where T : BaseEntity
    {
        private readonly WebApiDbContext _dbContext;

        public NotFoundAttribute(WebApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var id = 0;
            if (context.ActionArguments.ContainsKey("id"))
                id = (int) context.ActionArguments["id"];
            else
            {
                context.Result = new BadRequestObjectResult("id not provided");
                return;
            }

            var entity = _dbContext.Set<T>().SingleOrDefault(x => x.Id.Equals(id));
            if(entity == null)
                context.Result = new NotFoundResult();
        }
    }
}
