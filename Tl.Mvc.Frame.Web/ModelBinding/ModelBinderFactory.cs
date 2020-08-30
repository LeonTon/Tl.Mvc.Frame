using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class ModelBinderFactory : IModelBinderFactory
    {
        private IModelBinderProvider[] _modelBinderProviders;

        public ModelBinderFactory(IEnumerable<IModelBinderProvider> modelBinderProviders)
        {
            _modelBinderProviders = modelBinderProviders.ToArray();
        }

        public IModelBinder CreateModelBinder(ModelMetadata modelMetadata)
        {
            foreach (var provider in _modelBinderProviders)
            {
                var binder = provider.CreateModelBinder(modelMetadata);
                if (binder != null)
                {
                    return binder;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
