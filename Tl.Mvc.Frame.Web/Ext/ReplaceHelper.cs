using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;

namespace MvcApp.Controllers
{
internal static class ReplaceHelper
{
    private static MethodInfo replaceDictionaryMethod = typeof(ReplaceHelper).GetMethod("ReplaceDictionaryImpl", BindingFlags.Static | BindingFlags.NonPublic);
    private static MethodInfo replaceCollectionMethod = typeof(ReplaceHelper).GetMethod("ReplaceCollectionImpl", BindingFlags.Static | BindingFlags.NonPublic);

    public static void ReplaceCollection(Type collectionType, object collection, object newContents)
    {
        replaceCollectionMethod.MakeGenericMethod(new Type[] { collectionType }).Invoke(null, new object[] { collection, newContents });
    }

    public static void ReplaceDictionary(Type keyType, Type valueType, object dictionary, object newContents)
    {
        replaceDictionaryMethod.MakeGenericMethod(new Type[] { keyType, valueType }).Invoke(null, new object[] { dictionary, newContents });
    }

    private static void ReplaceDictionaryImpl<TKey, TValue>(IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<object, object>> newContents)
    {
        dictionary.Clear();
        foreach (KeyValuePair<object, object> pair in newContents)
        {
            TKey key = (TKey)pair.Key;
            TValue local2 = (TValue)((pair.Value is TValue) ? pair.Value : default(TValue));
            dictionary[key] = local2;
        }
    }
    private static void ReplaceCollectionImpl<T>(ICollection<T> collection, IEnumerable newContents)
    {
        collection.Clear();
        if (newContents != null)
        {
            foreach (object obj2 in newContents)
            {
                T item = (obj2 is T) ? ((T)obj2) : default(T);
                collection.Add(item);
            }
        }
    }
}
}