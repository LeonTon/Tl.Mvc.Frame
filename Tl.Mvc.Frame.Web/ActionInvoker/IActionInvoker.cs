using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web
{
    public interface IActionInvoker
    {
        Task InvokerAsync();
    }
}
