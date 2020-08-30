using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Reflection;
using System.Collections;

namespace MvcApp.Controllers
{
    public class DefaultModelBinder
    {
        public IValueProvider ValueProvider { get; private set; }
        public DefaultModelBinder(IValueProvider valueProvider)
        {
            this.ValueProvider = valueProvider;
        }
        public IEnumerable<object> GetParameterValues(ActionDescriptor actionDescriptor)
        {
            foreach (ParameterDescriptor parameterDescriptor in actionDescriptor.GetParameters())
            {
                string prefix = parameterDescriptor.BindingInfo.Prefix ?? parameterDescriptor.ParameterName;
                yield return GetParameterValue(parameterDescriptor, prefix);
            }
        }
        public object GetParameterValue(ParameterDescriptor parameterDescriptor, string prefix)
        {
            object parameterValue = BindModel(parameterDescriptor.ParameterType, prefix);
            if (null == parameterValue && string.IsNullOrEmpty(parameterDescriptor.BindingInfo.Prefix))
            {
                parameterValue = BindModel( parameterDescriptor.ParameterType, "");
            }
            return parameterValue ?? parameterDescriptor.DefaultValue;
        }
        public object BindModel(Type parameterType, string prefix)
        {
            if (!this.ValueProvider.ContainsPrefix(prefix))
            {
                return null;
            }

            ModelMetadata modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => null, parameterType);
            if (!modelMetadata.IsComplexType)
            {
                return this.ValueProvider.GetValue(prefix).ConvertTo(parameterType);
            }
            if (parameterType.IsArray)
            {
                return BindArrayModel(parameterType, prefix);
            }
            object model = CreateModel(parameterType);
            Type dictionaryType = ExtractGenericInterface(parameterType, typeof(IDictionary<,>));
            if (null != dictionaryType)
            {
                return BindDictionaryModel(prefix, model, dictionaryType);
            }

            Type enumerableType = ExtractGenericInterface(parameterType, typeof(IEnumerable<>));
            if (null != enumerableType)
            {
                return BindCollectionModel(prefix, model, enumerableType);
            }
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(parameterType))
            {                
                string key = prefix == "" ? property.Name : prefix + "." + property.Name;
                property.SetValue(model, BindModel(property.PropertyType, key));
            }
            return model;
        }

        private object BindCollectionModel(string prefix, object model, Type enumerableType)
        {
            List<object> list = new List<object>();
            bool numericIndex;
            IEnumerable<string> indexes = GetIndexes(prefix, out numericIndex);
            Type elementType = enumerableType.GetGenericArguments()[0];
            foreach (var index in indexes)
            {
                string indexPrefix = prefix + "[" + index + "]";
                if (!this.ValueProvider.ContainsPrefix(indexPrefix) && numericIndex)
                {
                    break;
                }
                list.Add(BindModel(elementType, indexPrefix));
            }
            if (list.Count == 0)
            {
                return null;
            }
            ReplaceHelper.ReplaceCollection(elementType, model, list);
            return model;
        }
        private object BindDictionaryModel(string prefix, object model, Type dictionaryType)
        {
            List<KeyValuePair<object, object>> list = new List<KeyValuePair<object, object>>();
            bool numericIndex;
            IEnumerable<string> indexes = GetIndexes(prefix, out numericIndex);
            Type[] genericArguments = dictionaryType.GetGenericArguments();
            Type keyType = genericArguments[0];
            Type valueType = genericArguments[1];

            foreach (var index in indexes)
            {
                string indexPrefix = prefix + "[" + index + "]";
                if (!this.ValueProvider.ContainsPrefix(indexPrefix) && numericIndex)
                {
                    break;
                }
                string keyPrefix = indexPrefix + ".Key";
                string valulePrefix = indexPrefix + ".Value";
                list.Add(new KeyValuePair<object, object>(BindModel(keyType, keyPrefix), BindModel(valueType, valulePrefix)));
            }
            if (list.Count == 0)
            {
                return null;
            }
            ReplaceHelper.ReplaceDictionary(keyType, valueType, model, list);
            return model;
        }
        private object BindArrayModel(Type parameterType, string prefix)
        {
            List<object> list = new List<object>();

            if (string.IsNullOrEmpty(prefix) && this.ValueProvider.ContainsPrefix(prefix))
            {
                IEnumerable enumerable = this.ValueProvider.GetValue(prefix).ConvertTo(parameterType) as IEnumerable;
                if (null != enumerable)
                {
                    foreach (var value in enumerable)
                    {
                        list.Add(value);
                    }
                }
            }      

            bool numericIndex;
            IEnumerable<string> indexes = GetIndexes(prefix, out numericIndex);
            foreach (var index in indexes)
            {
                string indexPrefix = prefix + "[" + index + "]";
                if (!this.ValueProvider.ContainsPrefix(indexPrefix) && numericIndex)
                {
                    break;
                }
                list.Add(BindModel(parameterType.GetElementType(), indexPrefix));
            }
            object[] array = (object[])Array.CreateInstance(parameterType.GetElementType(), list.Count);
            list.CopyTo(array);
            return array;
        }
        private IEnumerable<string> GetIndexes(string prefix, out bool numericIndex)
        { 
            string key = string.IsNullOrEmpty(prefix)?"index": prefix+"."+"index";
            ValueProviderResult result = this.ValueProvider.GetValue(key);
            if (null != result)
            {
                string[] indexes = result.ConvertTo(typeof(string[])) as string[];
                if (null != indexes)
                {
                    numericIndex = false;
                    return indexes;
                }
            }
            numericIndex = true;
            return GetZeroBasedIndexes();
        }
        private static IEnumerable<string> GetZeroBasedIndexes()
        {
            int iteratorVariable0 = 0;
            while (true)
            {
                yield return iteratorVariable0.ToString();
                iteratorVariable0++;
            }
        }
        private object CreateModel(Type modelType)
        {
            Type type = modelType;
            if (modelType.IsGenericType)
            {
                Type genericTypeDefinition = modelType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IDictionary<,>))
                {
                    type = typeof(Dictionary<,>).MakeGenericType(modelType.GetGenericArguments());
                }
                else if (((genericTypeDefinition == typeof(IEnumerable<>)) || (genericTypeDefinition == typeof(ICollection<>))) || (genericTypeDefinition == typeof(IList<>)))
                {
                    type = typeof(List<>).MakeGenericType(modelType.GetGenericArguments());
                }
            }
            return Activator.CreateInstance(type);
        }
        private Type ExtractGenericInterface(Type queryType, Type interfaceType)
        {
            Func<Type, bool> predicate = t => t.IsGenericType && (t.GetGenericTypeDefinition() == interfaceType);
            if (!predicate(queryType))
            {
                return queryType.GetInterfaces().FirstOrDefault<Type>(predicate);
            }
            return queryType;
        }
    }
}