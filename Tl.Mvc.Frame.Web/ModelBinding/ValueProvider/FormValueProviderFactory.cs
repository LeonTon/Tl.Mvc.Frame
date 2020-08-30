using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class FormValueProviderFactory : IValueProviderFactory
    {
        public IValueProvider CreateValueProvider(ActionContext actionContext)
        {
            var contentType = actionContext.HttpContext.Request.GetTypedHeaders().ContentType;
            var provider = contentType?.MediaType.Equals("application/x-www-form-urlencoded") == true
                ? new ValueProvider(actionContext.HttpContext.Request.Form)
                : new ValueProvider(new KeyValuePair<string, StringValues>[0]);

            return provider;
        }
    }
}
