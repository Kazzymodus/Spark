using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkEngine.Components
{
    class ArrayGrid : Grid
    {
        public ArrayGrid(Perspective perspective)
            : base(perspective)
        {

        }

        public ProtoEntity this[int x, int y]
        {
            get => Cells[x, y];
        }

        public ProtoEntity[,] Cells { get; }

        public Type[] TypeOrder { get; }
    }
}
