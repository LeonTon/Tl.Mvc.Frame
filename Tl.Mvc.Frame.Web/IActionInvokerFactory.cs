using System;
using System.Collections.Generic;
using System.Text;
using Tl.Mvc.Frame.Web;

namespace Tl.Mvc.Frame.Web
{
    public interface IActionInvokerFactory
    {
        IActionInvoker Create(ActionContext actionContext);
    }
}
