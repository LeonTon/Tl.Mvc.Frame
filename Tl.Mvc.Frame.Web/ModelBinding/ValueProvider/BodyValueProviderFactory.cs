using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class BodyValueProviderFactory :IValueProviderFactory
    {
        public IValueProvider CreateValueProvider(ActionContext actionContext)
        {
            throw new Exception();
            //return new ValueProvider(actionContext.HttpContext.Request.Body.);
        }
    }
}
