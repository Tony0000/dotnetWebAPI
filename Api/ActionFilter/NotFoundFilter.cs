using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.ActionFilter
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IWebApiDbContext _dbContext;
        
        public NotFoundFilter(IWebApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.ContainsKey("id"))
            {
                if (context.ActionArguments["id"] is int id)
                {
                    if (!_dbContext.Set<T>().AnyDynamic(x => $"x.Id == {id}"))
                    {
                        context.Result = new NotFoundResult();
                        return;
                    }
                }
            }

            await next();
        }
    }
}