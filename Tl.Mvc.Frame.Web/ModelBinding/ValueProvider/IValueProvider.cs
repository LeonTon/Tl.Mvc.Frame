using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public interface IValueProvider
    {
        bool TryGetValues(string name, out string[] values);

        bool ContainsPrefix(string prefix);
    }
}
