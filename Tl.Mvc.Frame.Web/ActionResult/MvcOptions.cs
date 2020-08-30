using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ActionResult
{
    public class MvcOptions
    {
        public IDictionary<Type, Func<object, ActionContext, IActionResult>> TypedActionResultConverters = new Dictionary<Type, Func<object, ActionContext, IActionResult>>();
    }
}
