namespace SparkEngine.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;

    public interface IHoverable
    {
        #region Properties

        Rectangle Bounds { get; }

        #endregion

        #region Methods

        void ExecuteHover();

        void ClearHover();

        #endregion
    }
}
