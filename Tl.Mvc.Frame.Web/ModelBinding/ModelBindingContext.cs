using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding
{
    public class ModelBindingContext
    {
        public ModelBindingContext(ActionContext actionContext, ModelMetadata modelMetadata, IValueProvider valueProvider, string modelName)
        {
            ActionContext = actionContext;
            ModelMetadata = modelMetadata;
            ValueProvider = valueProvider;
            ModelName = modelName;
        }

        public ActionContext ActionContext { get; }

        public ModelMetadata ModelMetadata { get; }

        public IValueProvider ValueProvider { get; }

        public string ModelName { get; }

        public object Model { get; private set; }

        public bool IsModelSet { get; private set; }

        public void Bind(object model)
        {
            Model = model;
            IsModelSet = true;
        }
    }
}
