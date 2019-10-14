namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Input;
    using SparkEngine.Rendering;
    using SparkEngine.States;
    using SparkEngine.Systems.Batching;

    public class BatchedGrid : Grid
    {
        
        #region Fields

        public CellBatch[] Batches { get; }

        #endregion

        #region Constructors

        public BatchedGrid(Perspective perspective, bool wrapAround)
            : base(perspective, 1, 1, wrapAround)
        {

        }

        #endregion      
        
    }   
}
