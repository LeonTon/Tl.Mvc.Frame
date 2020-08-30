using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.SimpleTypeBinder
{
    public class SimpleTypeModelBinder : IModelBinder
    {

        private readonly MethodInfo method = typeof(SimpleTypeModelBinder).GetMethod("GetDefaultValue").GetGenericMethodDefinition();

        public  Task BindAsync(ModelBindingContext modelBindingContext)
        {
            if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelName, out var values))
            {
                var value = Convert.ChangeType(values.Single(), modelBindingContext.ModelMetadata.ModelType);
                modelBindingContext.Bind(value);
               
            }
            modelBindingContext.Bind(method.MakeGenericMethod(modelBindingContext.ModelMetadata.ModelType).Invoke(null, new object[0]));
            return Task.CompletedTask;
        }

        public static T GetDefaultValue<T>()
        {
            return default;
        }
    }
}
