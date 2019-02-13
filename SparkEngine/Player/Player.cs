namespace SparkEngine.Player
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Dictionaries;
    using SparkEngine.IDs;
    using SparkEngine.Time;
    using SparkEngine.UI;

    public class Player
    {
        #region Fields

        private Resource[] resources = new Resource[ResourceIDs.Count];

        #endregion

        #region Constructor

        public Player()
        {
            for (int i = 0; i < ResourceIDs.Count; i++)
            {
                resources[i] = new Resource("Sod it");
            }
        }

        #endregion

        #region Properties

        public int Alignment { get; private set; }

        #endregion

        #region Methods

        public Resource GetResource(int resourceId)
        {
            return resources[resourceId];
        }

        public void ModifyResource(int resourceId, float amount)
        {
            resources[resourceId].Modify(amount);
        }

        public void ModifyAlignment(float amount)
        {
            Alignment += (int)amount;
        }

        internal void Update(GameTime gameTime)
        {
            TimeManager.Update(gameTime, this);
        }

        #endregion
    }
}
