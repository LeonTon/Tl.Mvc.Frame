using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class NullActionResult : IActionResult
    {
        public NullActionResult()
        {
        }

        public static NullActionResult Instance { get; } = new NullActionResult();

        public Task ExcuteResultAsync(ActionContext actionContext)
        {
            return Task.CompletedTask;
        }
    }
}
