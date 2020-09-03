using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.ComplexTypeBinder
{
    public class ComplexTypeModelBinder : IModelBinder
    {
        public async Task BindAsync(ModelBindingContext modelBindingContext)
        {
            var metaData = modelBindingContext.ModelMetadata;
            var model = Activator.CreateInstance(metaData.ModelType);
            foreach (var property in metaData.ModelType.GetProperties().Where(p => p.SetMethod != null))
            {
                var propertyMetaData = ModelMetadata.Create(property);
                var binderFactory = modelBindingContext.ActionContext.HttpContext.RequestServices.GetRequiredService<IModelBinderFactory>();
                var binder = binderFactory.CreateModelBinder(propertyMetaData);
                var modelName = string.IsNullOrWhiteSpace(modelBindingContext.ModelName)
                    ? property.Name
                    : $"{metaData.ModelName}.{property.Name}";
                var propertyContext = new ModelBindingContext(modelBindingContext.ActionContext, propertyMetaData, modelBindingContext.ValueProvider, modelName);
                await binder.BindAsync(propertyContext);
                if (propertyContext.IsModelSet)
                {
                    property.SetValue(model, propertyContext.Model);
                }

            }
            modelBindingContext.Bind(model);
        }
    }
}
