namespace SparkEngine.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Entity
    {
        #region Constructors

        public Entity(int id)
        {
            ID = id;
        }

        #endregion

        #region Properties

        public int ID { get; }

        #endregion
    }
}
