using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class ModelMetadata
    {
        public ModelMetadata(ParameterInfo parameterInfo, PropertyInfo propertyInfo)
        {
            ParameterInfo = parameterInfo;
            PropertyInfo = propertyInfo;
            ModelName = parameterInfo?.Name ?? propertyInfo?.Name;
            ModelType = parameterInfo?.ParameterType ?? propertyInfo?.PropertyType;
            IsSimpleType = TypeDescriptor.GetConverter(ModelType).CanConvertFrom(typeof(string));
        }

        public ParameterInfo ParameterInfo { get;}

        public PropertyInfo PropertyInfo { get; }

        public string ModelName { get; }

        public Type ModelType { get; }

        public bool IsSimpleType { get; }

        public static ModelMetadata Create(ParameterInfo parameterInfo)
       => new ModelMetadata(parameterInfo, null);

        public static ModelMetadata Create(PropertyInfo propertyInfo)
        => new ModelMetadata(null, propertyInfo);
    }
}
