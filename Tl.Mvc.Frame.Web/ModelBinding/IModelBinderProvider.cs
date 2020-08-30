using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public interface IModelBinderProvider
    {
        IModelBinder CreateModelBinder(ModelMetadata modelMetadata);
    }
}
