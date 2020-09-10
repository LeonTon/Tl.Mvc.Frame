using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.IEnumerableBinder
{
    public class IEnumerableBinderProvider : IModelBinderProvider
    {
        public IModelBinder CreateModelBinder(ModelMetadata modelMetadata)
        {
            if (modelMetadata.IsSimpleType)
            {
                return null;
            }
            return (modelMetadata.ParameterInfo?.GetCustomAttribute<FromFormAttribute>() != null
                   && modelMetadata.ParameterInfo.ParameterType.IsArray)
                   || modelMetadata.PropertyInfo.PropertyType.IsSubclassOf(typeof(IEnumerable))
                   
                ? new IEnumerableModelBinder()
                : null;
        }
    }
}
