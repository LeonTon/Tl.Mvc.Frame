using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tl.Mvc.Frame.Web.ModelBinding.ArrayBinder
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindAsync(ModelBindingContext modelBindingContext)
        {
            List<string> list = new List<string>();
            if (modelBindingContext.ValueProvider.TryGetValues(modelBindingContext.ModelMetadata.ModelName, out var values))
            {
                var stringList = JsonSerializer.Deserialize<List<string>>(values.Last());
                foreach (var str in stringList)
                {
                    list.Add(str);
                }

                
                //var enumerable = Convert.ChangeType(values.Last(), modelBindingContext.ModelMetadata.ModelType) as IEnumerable;
                //if (null != enumerable)
                //{
                //    foreach (var value in enumerable)
                //    {
                //        list.Add(value);
                //    }
                //}
            }

            //object[] array =(object[]) Array.CreateInstance(modelBindingContext.ModelMetadata.ModelType,list.Count);
            //for (var i=0;i<array.Length;i++ )
            //{
            //    object obj = list[i];
            //    array[i] = obj;
            //}

            modelBindingContext.Bind(list.ToArray());

            return Task.CompletedTask;
        }
    }
}
