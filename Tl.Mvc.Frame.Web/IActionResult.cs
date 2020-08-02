using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public interface IActionResult
    {
        Task ExcuteResultAsync(ActionContext actionContext);
    }
}
