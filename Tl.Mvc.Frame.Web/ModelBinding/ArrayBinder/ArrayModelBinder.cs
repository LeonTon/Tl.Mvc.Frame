using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.ArrayBinder
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindAsync(ModelBindingContext modelBindingContext)
        {
            List<object> list = new List<object>();

            if (string.IsNullOrEmpty(prefix) && this.ValueProvider.ContainsPrefix(prefix))
            {
                IEnumerable enumerable = this.ValueProvider.GetValue(prefix).ConvertTo(parameterType) as IEnumerable;
                if (null != enumerable)
                {
                    foreach (var value in enumerable)
                    {
                        list.Add(value);
                    }
                }
            }

            bool numericIndex;
            IEnumerable<string> indexes = GetIndexes(prefix, out numericIndex);
            foreach (var index in indexes)
            {
                string indexPrefix = prefix + "[" + index + "]";
                if (!this.ValueProvider.ContainsPrefix(indexPrefix) && numericIndex)
                {
                    break;
                }
                list.Add(BindModel(parameterType.GetElementType(), indexPrefix));
            }
            object[] array = (object[])Array.CreateInstance(parameterType.GetElementType(), list.Count);
            list.CopyTo(array);
            return array;
            throw new NotImplementedException();
        }
    }
}
