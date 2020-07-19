using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tl.Mvc.Frame.Web
{
    public class ActionDescriptor
    {
        public ActionDescriptor(MethodInfo methodInfo, RouteInfo routeInfo)
        {
            MethodInfo = methodInfo;
            RouteInfo = routeInfo;

            ActionName = methodInfo.Name;
            ActionName = ActionName.Substring(0, ActionName.Length - "Async".Length);

            ControllerName = methodInfo.DeclaringType.Name;
            ControllerName = ControllerName.Substring(0, ControllerName.Length - "Controller".Length);
        }

        public MethodInfo MethodInfo { get; set; }

        public RouteInfo RouteInfo { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }
    }

    public class RouteInfo
    {
        public RouteInfo(string name, int? order, string template)
        {
            Name = name;
            Order = order;
            Template = template;
        }

        public string Name { get; set; }

        public int? Order { get; set; }

        public string Template { get; set; }
    }
}
