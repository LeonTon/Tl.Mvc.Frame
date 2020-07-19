using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tl.Mvc.Frame.Web
{
    public class ActionContext
    {
        public ActionContext(HttpContext httpContext, ActionDescriptor actionDiscriptor)
        {
            HttpContext = httpContext;
            ActionDiscriptor = actionDiscriptor;
        }

        public HttpContext HttpContext { get; set; }


        public ActionDescriptor ActionDiscriptor { get; set; }
    }
}
