using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.ArrayBinder
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindAsync(ModelBindingContext modelBindingContext)
        {
            List<object> list = new List<object>();
            if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelMetadata.ModelName, out var values))
            {
                var obj = JsonSerializer.Deserialize<List<string>>(values.Last());
                var b = Convert.ChangeType(obj, modelBindingContext.ModelMetadata.ParameterInfo.ParameterType);
                var enumerable = Convert.ChangeType(obj, modelBindingContext.ModelMetadata.ModelType) as IEnumerable;
                if (null != enumerable)
                {
                    foreach (var value in enumerable)
                    {
                        list.Add(value);
                    }
                }
            }

            object[] array =(object[]) Array.CreateInstance(modelBindingContext.ModelMetadata.ModelType,list.Count);
            list.CopyTo(array);
            modelBindingContext.Bind(array);

            return Task.CompletedTask;
        }

        private IEnumerable<string> GetIndexes(ModelBindingContext modelBindingContext, string prefix, out bool numericIndex)
        {
            string key = string.IsNullOrEmpty(prefix) ? "index" : prefix + "." + "index";
            if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelName, out var values))
            {
                if (null != values)
                {
                    string[] indexes = Convert.ChangeType(values, typeof(string[])) as string[];
                    if (null != indexes)
                    {
                        numericIndex = false;
                        return indexes;
                    }
                }
            }
            numericIndex = true;
            return GetZeroBasedIndexes();
        }
        private static IEnumerable<string> GetZeroBasedIndexes()
        {
            int iteratorVariable0 = 0;
            while (true)
            {
                yield return iteratorVariable0.ToString();
                iteratorVariable0++;
            }
        }
    }
}
