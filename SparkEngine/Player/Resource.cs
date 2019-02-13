namespace SparkEngine.Player
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Resource
    {
        #region Constructors

        public Resource(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        public string Name { get; }

        public int Amount { get; private set; }

        #endregion

        #region Methods

        public void Modify(float amount)
        {
            Amount += (int)amount;
        }

        #endregion
    }
}
