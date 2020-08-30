using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public async Task InvokerAsync()
        {
            var controllerType = ActionContext.ActionDiscriptor.MethodInfo.DeclaringType;
            var serviceProvider = ActionContext.HttpContext.RequestServices;
            var controller = (Controller)ActivatorUtilities.CreateInstance(serviceProvider, controllerType);
            controller.ActionContext = ActionContext;

            var excutor = serviceProvider.GetRequiredService<IActionMethodExcutor>();

            var result = excutor.Convert(controller, ActionContext, new object[0]);
            var converter = serviceProvider.GetRequiredService<IActionResultConvertor>();
            var actionResult = converter.Convert(result, ActionContext);
            await actionResult.ExcuteResultAsync(ActionContext);
        }

    }
}
