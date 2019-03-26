using System;
using System.Collections.Generic;
using Google.Protobuf;
using System.Collections;

public static class MessageFactory
{
    private static Dictionary<Type, Stack<object>> mMessagePool = new Dictionary<Type, Stack<object>>();
    internal static T GetInstance<T>(Type t)
    {
        if (t.IsEnum) return Activator.CreateInstance<T>();
        Stack<object> stack;
        mMessagePool.TryGetValue(t,out stack);
        if (stack != null && stack.Count > 0) return (T)stack.Pop();
        return Activator.CreateInstance<T>();
    }
    public static void RecycleInstance(object obj)
    {
        Type t = obj.GetType();
        if (t.IsEnum) return;
            
        if (!typeof(IMessage).IsAssignableFrom(t)) throw new ArgumentException("Instance must derived from IMessage:",obj.ToString());
        //析构所有引用类型的成员
        var properties = t.GetProperties();
        foreach(var finfo in properties)
        {
            if (!typeof(IList).IsAssignableFrom(finfo.PropertyType)) continue;

            var valueList = finfo.GetValue(obj) as IList;

            for (int i= valueList.Count-1;i>=0; i--)
            {
                object mes = valueList[i];
                valueList.RemoveAt(i);
                RecycleInstance(mes);
            }
        }
        if (!mMessagePool.ContainsKey(t) || mMessagePool[t] == null) mMessagePool[t] = new Stack<object>();
        mMessagePool[t].Push(obj);
    }
}

