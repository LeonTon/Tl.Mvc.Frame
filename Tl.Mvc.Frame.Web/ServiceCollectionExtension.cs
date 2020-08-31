using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tl.Mvc.Frame.Web.ModelBinding;
using Tl.Mvc.Frame.Web.ModelBinding.SimpleTypeBinder;

namespace Tl.Mvc.Frame.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMvcController(this IServiceCollection services)
        {
            return services.AddSingleton<IActionDescriptorProvider, ActionDescriptorProvider>()
                           .AddSingleton<IActionInvokerFactory, ActionInvokerFactory>()
                           .AddSingleton<IActionResultConvertor, ActionResultConvertor>()
                           .AddSingleton<IActionMethodExcutor, ActionMethodExcutor>()
                           .AddSingleton<IActionResultConvertor, ActionResultConvertor>()
                           .AddSingleton<IModelBinderFactory, ModelBinderFactory>()
                           .AddSingleton<IValueProviderFactory, FormValueProviderFactory>()
                           .AddSingleton<IValueProviderFactory, QueryStringValueProviderFactory>()
                           .AddSingleton<IModelBinderProvider, SimpleTypeBinderProvider>();

        }

        public static IEndpointRouteBuilder MapMvcControllers(this IEndpointRouteBuilder builder)
        {
            var source = ActivatorUtilities.CreateInstance<MvcEndpointDataSource>(builder.ServiceProvider);
            builder.DataSources.Add(source);
            return builder;
        }
    }
}
