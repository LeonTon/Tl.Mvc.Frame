using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class ValueProvider : IValueProvider
    {

        private IEnumerable<KeyValuePair<string, StringValues>> _data;

        public ValueProvider(IEnumerable<KeyValuePair<string, StringValues>> data)
        {
            _data = data;
        }

        public bool ContainsPrefix(string prefix)
        {
            return _data.Any(item => item.Key.Contains(prefix));
        }

        public bool TryGetValues(string name, out string[] values)
        {
            values = _data.FirstOrDefault(item => item.Key == name).Value;
            return values != default(StringValues);
        }
    }
}
