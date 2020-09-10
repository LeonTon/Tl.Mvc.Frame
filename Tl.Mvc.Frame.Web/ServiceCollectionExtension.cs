using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Tl.Mvc.Frame.Web.ModelBinding;
using Tl.Mvc.Frame.Web.ModelBinding.ArrayBinder;
using Tl.Mvc.Frame.Web.ModelBinding.ComplexTypeBinder;
using Tl.Mvc.Frame.Web.ModelBinding.SimpleTypeBinder;
using Tl.Mvc.Frame.Web.ModelBinding.IEnumerableBinder;

namespace Tl.Mvc.Frame.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMvcController(this IServiceCollection services)
        {
            return services.AddSingleton<IActionDescriptorProvider, ActionDescriptorProvider>()
                           .AddSingleton<IActionInvokerFactory, ActionInvokerFactory>()
                           .AddSingleton<IActionResultConvertor, ActionResultConvertor>()
                           .AddSingleton<IActionMethodExecutor, ActionMethodExecutor>()
                           .AddSingleton<IActionResultConvertor, ActionResultConvertor>()

                           .AddSingleton<IModelBinderFactory, ModelBinderFactory>()
                           .AddSingleton<IValueProviderFactory, FormValueProviderFactory>()
                           .AddSingleton<IValueProviderFactory, QueryStringValueProviderFactory>()
                           .AddSingleton<IModelBinderProvider, SimpleTypeBinderProvider>()
                           .AddSingleton<IModelBinderProvider, ComplexTypeBinderProvider>()
                           .AddSingleton<IModelBinderProvider, ArrayBinderProvider>()
                           .AddSingleton<IModelBinderProvider, IEnumerableBinderProvider>()
                           ;


        }

        public static IEndpointRouteBuilder MapMvcControllers(this IEndpointRouteBuilder builder)
        {
            var source = ActivatorUtilities.CreateInstance<MvcEndpointDataSource>(builder.ServiceProvider);
            builder.DataSources.Add(source);
            return builder;
        }
    }
}
