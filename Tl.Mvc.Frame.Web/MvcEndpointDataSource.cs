using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tl.Mvc.Frame.Web;

namespace Tl.Mvc.Frame.Web
{
    public class MvcEndpointDataSource : EndpointDataSource
    {
        private readonly Lazy<IReadOnlyList<Endpoint>> _endpoints;

        private readonly IActionInvokerFactory _actionInvokerFactory;


        public MvcEndpointDataSource(IActionInvokerFactory actionInvokerFactory,
            IActionDescriptorProvider actionDiscriptorProvider,
            RoutePatternTransformer transformer)
        {
            _actionInvokerFactory = actionInvokerFactory;

            _endpoints = new Lazy<IReadOnlyList<Endpoint>>(Create());

            List<Endpoint> Create()
            {
                var Descriptors = actionDiscriptorProvider.GetActionDescriptors;
                return Descriptors.Select(CreateEndpoint).ToList();
            }

            Endpoint CreateEndpnaoint(ActionDescriptor actionDiscriptor)
            {
                var routePattern = RoutePatternFactory.Parse(actionDiscriptor.RouteInfo.Template);
                var newRoutePattern = transformer.SubstituteRequiredValues(routePattern, new Dictionary<string, string>
                {
                    ["controller"] = actionDiscriptor.ControllerName,
                    ["action"] = actionDiscriptor.ActionName
                });
                routePattern = routePattern ?? newRoutePattern;
                var endPointBuilder = new RouteEndpointBuilder(InvokeAsync, routePattern, actionDiscriptor.RouteInfo.Order ?? 0);
                endPointBuilder.Metadata.Add(actionDiscriptor);
                return endPointBuilder.Build();
            }


        }

        public override IReadOnlyList<Endpoint> Endpoints => _endpoints.Value;

        public override IChangeToken GetChangeToken() => NullChangeToken.Singleton;

        private Task InvokeAsync(HttpContext httpContext)
        {
            var endPoint = httpContext.GetEndpoint();
            var actionDiscriptor = endPoint.Metadata.GetMetadata<ActionDescriptor>();
            var actionContext = new ActionContext(httpContext, actionDiscriptor);
            var actionInvoker = _actionInvokerFactory.Create(actionContext);
            return actionInvoker.InvokerAsync();

        }
    }
}
