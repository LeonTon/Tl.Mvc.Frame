using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class ActionMethodExcutor : IActionMethodExcutor
    {
        public async Task ExcuteAsync(Controller controller)
        {
            var serviceProvider = controller.ActionContext.HttpContext.RequestServices;
            var result = controller.ActionContext.ActionDiscriptor.MethodInfo.Invoke(controller, Array.Empty<object>());
            var convertor = serviceProvider.GetRequiredService<IActionResultConvertor>();
            var actionResult = await ConvertActionResultAsync(result, controller.ActionContext.ActionDiscriptor.MethodInfo.ReturnType, convertor);
            await actionResult.ExcuteResultAsync(controller.ActionContext);
        }




        private async Task<IActionResult> ConvertActionResultAsync(object returnValue, Type returnType, IActionResultConvertor convertor)
        {
            try
            {
                //Null
                if (returnValue == null || returnType == typeof(Task))
                {
                  
                    return  NullActionResult.Instance;
                }

                if (returnValue is Task<ContentResult> contentResult)
                {
                    return await contentResult;
                }

                if (returnValue is Task<JsonResult> jsonResult)
                {
                    return await jsonResult;
                }

                if (returnValue.GetType().BaseType == typeof(Task))
                {
                    var declaredType = returnType.GenericTypeArguments.Single();
                    
                    //returnValue =await Task<declaredType.GetType()>returnType;

                    
                }
                return await Task.FromResult(convertor.Convert(returnValue, returnType));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
