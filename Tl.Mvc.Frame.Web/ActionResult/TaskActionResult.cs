using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ActionResult
{
    public class TaskActionResult : IActionResult
    {

        public TaskActionResult(Task task)
        {
            Task = task;
        }

        public Task Task { get; }


        public Task ExcuteResultAsync(ActionContext actionContext)
        {
            return Task;
        }
    }


    public class TaskActionResult<T> : IActionResult
    {
        private readonly IDictionary<Type, Func<object, ActionContext, IActionResult>> _mapping;

        public Task<T> Task { get; }

        public TaskActionResult(Task<T> task, IDictionary<Type, Func<object, ActionContext, IActionResult>> mapping)
        {
            Task = task;
            _mapping = mapping;
        }

        public async Task ExcuteResultAsync(ActionContext actionContext)
        {
            var result = await Task;
            if (result == null)
            {
                return;
            }

            if (result is IActionResult actionResult)
            {
                await actionResult.ExcuteResultAsync(actionContext);
                return;
            }

            if (_mapping.TryGetValue(result.GetType(), out var convert))
            {
                await convert(result, actionContext).ExcuteResultAsync(actionContext);
                return;
            }

            await new ContentResult(result.ToString(), "text/plain").ExcuteResultAsync(actionContext);
        }
    }
}
