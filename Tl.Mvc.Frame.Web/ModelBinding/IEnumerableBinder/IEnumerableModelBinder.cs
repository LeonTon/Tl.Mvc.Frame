using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.IEnumerableBinder
{
    public class IEnumerableModelBinder : IModelBinder
    {
        public Task BindAsync(ModelBindingContext modelBindingContext)
        {
            IList list = null;
            if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelMetadata.ModelName, out var values))
            {
                var array = (IEnumerable<object>)JsonSerializer.Deserialize(values.Last(), modelBindingContext.ModelMetadata.ModelType);

                Type elementType = modelBindingContext.ModelMetadata.ModelType.GetGenericArguments()[0];

                if (array != null && array.Count() > 0)
                {
                    list = Array.CreateInstance(elementType, array.Count());
                    for (int i = 0; i < array.Count(); i++)
                    {
                        var convertResult = Convert.ChangeType(array.ToArray()[i], elementType);
                        list[i] = convertResult;
                    }
                }

            }
            modelBindingContext.Bind(list);

            return Task.CompletedTask;
        }
    }
}
