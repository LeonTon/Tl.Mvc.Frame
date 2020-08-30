using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tl.Mvc.Frame.Web.ActionResult;

namespace Tl.Mvc.Frame.Web
{
    public class ActionResultConvertor : IActionResultConvertor
    {
        private readonly IDictionary<Type, Func<object, ActionContext, IActionResult>> _mapping;
        public ActionResultConvertor(IOptions<MvcOptions> options)
        {
            _mapping = options.Value.TypedActionResultConverters;
        }


        public IActionResult Convert(object result, ActionContext actionContext)
        {
            if (result is IActionResult actionResult)
            {
                return actionResult;
            }

            //void
            if (result == null)
            {
                return NullActionResult.Instance;
            }

            //Task<T>
            var resultType = result.GetType();
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var t = resultType.GetGenericArguments()[0];
                return (IActionResult)Activator.CreateInstance(typeof(TaskActionResult<>).MakeGenericType(t), result, _mapping);
            }


            //Task
            if (result is Task task)
            {
                return new TaskActionResult(task);
            }

            return _mapping.TryGetValue(resultType, out var converter) ?
                converter(result, actionContext) :
                new ContentResult(result.ToString(), "text/plain");
        }
    }
}
