using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tl.Mvc.Frame.Web.ModelBinding.ComplexTypeBinder;

namespace Tl.Mvc.Frame.Web.ModelBinding.ArrayBinder
{
    public class ArrayBinderProvider : IModelBinderProvider
    {
        public IModelBinder CreateModelBinder(ModelMetadata modelMetadata)
        {
            if (modelMetadata.IsSimpleType)
            {
                return null;
            }
            return (modelMetadata.ParameterInfo?.GetCustomAttribute<FromFormAttribute>() != null
                   && modelMetadata.ParameterInfo.ParameterType.IsArray)
                   ||modelMetadata.PropertyInfo.PropertyType.IsArray
                ? new ArrayModelBinder()
                : null;
        }
    }
}
