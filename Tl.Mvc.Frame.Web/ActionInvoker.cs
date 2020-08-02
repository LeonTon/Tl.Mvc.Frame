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

            var result = ActionContext.ActionDiscriptor.MethodInfo.Invoke(controller, Array.Empty<object>());
            var convertor = serviceProvider.GetRequiredService<IActionResultConvertor>();
            var actionResult = await ToActionResultAsync(result, ActionContext.ActionDiscriptor.MethodInfo.ReturnType, convertor);
            await actionResult.ExcuteResultAsync(ActionContext);
            //return result is Task task ? task : Task.CompletedTask;
        }



        private async Task<IActionResult> ToActionResultAsync(object returnValue, Type returnType, IActionResultConvertor convertor)
        {
            //Null
            if (returnValue == null || returnType == typeof(Task))
            {
                return await Task.FromResult<IActionResult>(new NullActionResult().Instance);
            }

            //IActionResult
            if (returnValue is Task<ContentResult> contentResult)
            {
                return await contentResult;
            }

            //IActionResult
            if (returnValue is Task<JsonResult> jsonResult)
            {
                return await jsonResult;
            }

            
            return await Task.FromResult(convertor.Convert(returnValue, returnType));
        }

    }
}
