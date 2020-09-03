using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.ArrayBinder
{
    public class ArrayModelBinder : IModelBinder
    {
        public async Task BindAsync(ModelBindingContext modelBindingContext)
        {
            List<object> list = new List<object>();

            if (string.IsNullOrEmpty(modelBindingContext.ModelName)
                && modelBindingContext.ValueProvider.ContainsPrefix(modelBindingContext.ModelName))
            {
                if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelName, out var values))
                {
                    var enumerable = Convert.ChangeType(values.Last(), modelBindingContext.ModelMetadata.ModelType) as IEnumerable;
                    if (null != enumerable)
                    {
                        foreach (var value in enumerable)
                        {
                            list.Add(value);
                        }
                    }
                }


            }

            bool numericIndex;
            IEnumerable<string> indexes = GetIndexes(modelBindingContext, modelBindingContext.ModelName, out numericIndex);
            foreach (var index in indexes)
            {
                string indexPrefix = modelBindingContext.ModelName + "[" + index + "]";
                if (!modelBindingContext.ValueProvider.ContainsPrefix(indexPrefix) && numericIndex)
                {
                    break;
                }
                list.Add(BindModel(modelBindingContext.ModelMetadata.ModelType.GetElementType(), indexPrefix));
            }
            object[] array = (object[])Array.CreateInstance(modelBindingContext.ModelMetadata.ModelType, list.Count);
            list.CopyTo(array);
            await binder.BindAsync(propertyContext);
            if (propertyContext.IsModelSet)
            {
                property.SetValue(model, propertyContext.Model);
            }
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
