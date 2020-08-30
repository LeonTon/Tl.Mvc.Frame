using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class CompositeValueProvider: IValueProvider
    {
        private readonly IValueProvider[] _valueProviders;

        public CompositeValueProvider(IValueProvider[] valueProviders)
        {
            _valueProviders = valueProviders;
        }

        public bool ContainsPrefix(string prefix)
        {
            return _valueProviders.Any(provider => provider.ContainsPrefix(prefix));
        }

        public bool TryGetValues(string name, out string[] values)
        {
            foreach (var provider in _valueProviders)
            {
                if (provider.TryGetValues(name, out values))
                {
                    return true;
                }
            }

            values = default;
            return false;
        }
    }
}
