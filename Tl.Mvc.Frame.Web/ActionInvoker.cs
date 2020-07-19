using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class ActionInvoker : IActionInvoker
    {
        public ActionInvoker(ActionContext actionContext)
        {
            ActionContext = actionContext;
        }

        public ActionContext ActionContext { get; }

        public Task InvokerAsync()
        {
            var controllerType = ActionContext.ActionDiscriptor.MethodInfo.DeclaringType;
            var serviceProvider = ActionContext.HttpContext.RequestServices;
            var controller = (Controller)ActivatorUtilities.CreateInstance(serviceProvider, controllerType);
            controller.ActionContext = ActionContext;

            var result = ActionContext.ActionDiscriptor.MethodInfo.Invoke(controller, Array.Empty<object>());
            return result is Task task ? task : Task.CompletedTask;
        }
    }
}
