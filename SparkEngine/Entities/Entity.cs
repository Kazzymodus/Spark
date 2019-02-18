namespace SparkEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SparkEngine.Components;

    public abstract class Entity
    {
        public List<Component> Components { get; } = new List<Component>();
    }
}
