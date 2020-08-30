using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.SimpleTypeBinder
{
    public class SimpleTypeBinderProvider : IModelBinderProvider
    {
        public IModelBinder CreateModelBinder(ModelMetadata modelMetadata)
        {
            return modelMetadata.IsSimpleType ? new SimpleTypeModelBinder() : null;
        }
    }
}
