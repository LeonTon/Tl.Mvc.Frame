using System;
using System.Collections.Generic;
using System.Text;

namespace Tl.Mvc.Frame.Web
{
    public interface IActionDescriptorProvider
    {
        IEnumerable<ActionDescriptor> GetActionDescriptors { get; }
    }
}
