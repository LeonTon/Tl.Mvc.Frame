using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public class ActionResultConvertor : IActionResultConvertor
    {
        public IActionResult Convert(object value, Type returnType)
        {
            return new ContentResult(value.ToString(), "text/plain");
        }
    }
}
