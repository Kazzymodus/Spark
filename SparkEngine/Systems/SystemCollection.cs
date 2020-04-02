namespace SparkEngine.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SystemCollection : KeyedCollection<Type, ComponentSystem>
    {
        protected override Type GetKeyForItem(ComponentSystem item)
        {
            return item.GetType();
        }
    }
}
