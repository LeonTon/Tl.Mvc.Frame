using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class ActionDescriptorProvider : IActionDescriptorProvider
    {
        private readonly Lazy<IEnumerable<ActionDescriptor>> _actionDescriptorsAccessor;

        IEnumerable<ActionDescriptor> IActionDescriptorProvider.GetActionDescriptors => _actionDescriptorsAccessor.Value;

        public ActionDescriptorProvider(IHostEnvironment hostEnvironment)
        {
            var assembly = Assembly.Load(new AssemblyName(hostEnvironment.ApplicationName));
            _actionDescriptorsAccessor = new Lazy<IEnumerable<ActionDescriptor>>(() => GetActionDescriptors(assembly));

        }


        private IEnumerable<ActionDescriptor> GetActionDescriptors(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => IsController(t)).SelectMany(GetActionDescriptors);

            bool IsController(Type type)
            {
                return type.IsPublic && !type.IsAbstract && type.Name.EndsWith("Controller");
            }

        }

        private IEnumerable<ActionDescriptor> GetActionDescriptors(Type controllerType)
        {
            return controllerType.GetMethods().Where(IsAction).SelectMany(GetActionDescriptors);

            static bool IsAction(MethodInfo method)
            {
                return method.IsPublic && !method.IsAbstract && method.GetCustomAttributes().OfType<IRouteTemplateProvider>().Any();
            }

        }

        private IEnumerable<ActionDescriptor> GetActionDescriptors(MethodInfo method)
        {
            return method.GetCustomAttributes().OfType<IRouteTemplateProvider>()
                .Select(item => new ActionDescriptor(method, new RouteInfo(item.Name, item.Order, item.Template)));
        }
    }
}
