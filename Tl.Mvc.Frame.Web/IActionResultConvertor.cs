using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public interface IActionResultConvertor
    {
        IActionResult Convert(object value, Type returnType);
    }
}
