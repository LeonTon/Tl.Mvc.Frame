using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
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


            if ((modelMetadata.ParameterInfo?.ParameterType.IsArray ?? false)
                || (modelMetadata.PropertyInfo?.PropertyType.IsArray ?? false))
            {
                return new ArrayModelBinder();
            }

            return null;
        }
    }
}
