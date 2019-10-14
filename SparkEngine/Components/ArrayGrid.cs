namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SparkEngine.Systems.Batching;

    public class ArrayGrid : Grid
    {
        public ArrayGrid(Perspective perspective, ProtoEntity[,] grid, bool wrapAround, bool? isHomogenous = null)
            : base(perspective, grid.GetLength(0), grid.GetLength(1), wrapAround)
        {
            Cells = grid;
            IsHomogenous = isHomogenous;
        }

        public ProtoEntity this[int x, int y]
        {
            get => Cells[x, y];
        }

        public ProtoEntity[,] Cells { get; }
        
        public bool? IsHomogenous { get; private set; }

        public bool DetermineHomogenity(Type[] componentTypes)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!((ComponentBatch)Cells[x, y].Components).ContainsAll(componentTypes))
                    {
                        return (bool)(IsHomogenous = false);
                    }
                }
            }

            return (bool)(IsHomogenous = true);
        }
    }
}
