using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class ActionMethodExecutor : IActionMethodExecutor
    {
        public object Convert(object controller, ActionContext actionContext, object[] arguments)
        {
            return actionContext.ActionDiscriptor.MethodInfo.Invoke(controller, arguments);
        }

    }
}
