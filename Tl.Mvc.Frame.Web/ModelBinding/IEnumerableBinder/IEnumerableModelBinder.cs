using System;
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
            List<string> list = new List<string>();
            if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelMetadata.ModelName, out var values))
            {
                var stringList = JsonSerializer.Deserialize<List<string>>(values.Last());
                foreach (var str in stringList)
                {
                    list.Add(str);
                }

            }
            modelBindingContext.Bind(list);

            return Task.CompletedTask;
        }
    }
}
