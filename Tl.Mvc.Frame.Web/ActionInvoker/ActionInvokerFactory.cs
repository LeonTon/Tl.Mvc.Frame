using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class ActionInvokerFactory : IActionInvokerFactory
    {
        public IActionInvoker Create(ActionContext actionContext)
        {
            return new ActionInvoker(actionContext);
        }
    }
}
