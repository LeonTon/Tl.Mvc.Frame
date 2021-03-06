﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.ArrayBinder
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindAsync(ModelBindingContext modelBindingContext)
        {
            IList array = null;

            if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelMetadata.ModelName, out var values))
            {
                Type elementType = modelBindingContext.ModelMetadata.ModelType.GetElementType();

                if (values.Length == 1)
                {
                    var list = (object[]) JsonSerializer.Deserialize(values.Last(),
                        modelBindingContext.ModelMetadata.ModelType);
                    if (list != null && list.Length > 0)
                    {
                        array = Array.CreateInstance(elementType, list.Length);
                        for (int i = 0; i < list.Length; i++)
                        {
                            var convertResult = Convert.ChangeType(list.GetValue(i), elementType);
                            array[i] = convertResult;
                        }
                    }
                }
                else
                {
                    array = Array.CreateInstance(elementType, values.Length);
                    for (int i = 0; i < values.Length; i++)
                    {
                        var convertResult = Convert.ChangeType(values.GetValue(i), elementType);
                        array[i] = convertResult;
                    }
                }

            }

            modelBindingContext.Bind(array);

            return Task.CompletedTask;
        }
    }
}
