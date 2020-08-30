using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Tl.Mvc.Frame.Web.ModelBinding;

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

            var valueFactories = serviceProvider.GetServices<IValueProviderFactory>();
            var valueProviders = valueFactories.Select(item => item.CreateValueProvider(ActionContext)).ToArray();
            var valueProvider = new CompositeValueProvider(valueProviders);
            var paramters = ActionContext.ActionDiscriptor.MethodInfo.GetParameters();
            var arguments = new object[paramters.Length];
            var modelBinderFactory = serviceProvider.GetRequiredService<IModelBinderFactory>();
            for (int index = 0; index < arguments.Length; index++)
            {
                var metadata = ModelMetadata.Create(paramters[index]);
                var binder = modelBinderFactory.CreateModelBinder(metadata);
                var binderContext = valueProvider.ContainsPrefix(metadata.ModelName)
                    ? new ModelBindingContext(ActionContext, metadata, valueProvider, metadata.ModelName)
                    : new ModelBindingContext(ActionContext, metadata, valueProvider, "");
                await binder.BindAsync(binderContext);
                arguments[index] = binderContext.Model;
            }

            var result = excutor.Convert(controller, ActionContext, arguments);
            //var result = excutor.Convert(controller, ActionContext, new object[0]);
            var converter = serviceProvider.GetRequiredService<IActionResultConvertor>();
            var actionResult = converter.Convert(result, ActionContext);
            await actionResult.ExcuteResultAsync(ActionContext);
        }

    }
}
