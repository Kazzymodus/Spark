namespace SparkEngine.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public interface IClickable
    {
        #region Properties

        Rectangle Bounds { get; }

        #endregion

        #region Methods

        void ExecuteClick();

        #endregion
    }
}
