namespace SparkEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Entity
    {
        public Entity(int id)
        {
            ID = id;
        }

        public int ID { get; }
    }
}
