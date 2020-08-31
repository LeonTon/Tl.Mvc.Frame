using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public interface IActionMethodExecutor
    {
        object Convert(object controller, ActionContext actionContext, object[] arguments);
    }
}
