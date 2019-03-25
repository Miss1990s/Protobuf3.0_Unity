using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.Protobuf.Examples.AddressBook
{
    public static class MessageFactory
    {
        private static Dictionary<Type, Stack<object>> mMessagePool = new Dictionary<Type, Stack<object>>();
        internal static T GetInstance<T>(Type t)
        {
            if (t.IsEnum) return Activator.CreateInstance<T>();
            var stack = mMessagePool[t];
            if (stack != null && stack.Count > 0) return (T)stack.Pop();
            return Activator.CreateInstance<T>();
        }
        public static void RecycleInstance<T>(T obj)
        {
            Type t = obj.GetType();
            if (t.IsEnum) return;
            if (!t.IsSubclassOf(typeof(IMessage))) throw new ArgumentException("Instance must derived from IMessage:",obj.ToString());
            //析构所有引用类型的成员
            throw new NotImplementedException();
            if (mMessagePool[t] == null) mMessagePool[t] = new Stack<object>();
            mMessagePool[t].Push(obj);
        }
    }
}
