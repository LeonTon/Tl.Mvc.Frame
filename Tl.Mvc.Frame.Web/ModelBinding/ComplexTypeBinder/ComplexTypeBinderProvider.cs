using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.ComplexTypeBinder
{
    public class ComplexTypeBinderProvider : IModelBinderProvider
    {
        public IModelBinder CreateModelBinder(ModelMetadata modelMetadata)
        {
            if (modelMetadata.IsSimpleType)
            {
                return null;
            }
            return modelMetadata.ParameterInfo?.GetCustomAttribute<FromFormAttribute>() != null
                   && !modelMetadata.ParameterInfo.ParameterType.IsArray
                ? new ComplexTypeModelBinder()
                : null;
        }
    }
}
