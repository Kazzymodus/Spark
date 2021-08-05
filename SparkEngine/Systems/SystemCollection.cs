using System;
using System.Collections.ObjectModel;

namespace SparkEngine.Systems
{
    public class SystemCollection : KeyedCollection<Type, ComponentSystem>
    {
        protected override Type GetKeyForItem(ComponentSystem item)
        {
            return item.GetType();
        }
    }
}