using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class QueryStringValueProviderFactory : IValueProviderFactory
    {
        public IValueProvider CreateValueProvider(ActionContext actionContext)
        {
            return new ValueProvider(actionContext.HttpContext.Request.Query);
        }
    }
}
