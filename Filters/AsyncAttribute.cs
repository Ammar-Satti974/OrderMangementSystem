using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace WebApiProject.Filters
{
    public class AsyncAttribute : Attribute, IAsyncActionFilter
    {
        private string _actionName;
        public AsyncAttribute(string actionName)
        {
            _actionName = actionName;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Debug.WriteLine($"Before {_actionName}");
            await next();
            Debug.WriteLine($"After {_actionName}");
        }
    }
}
