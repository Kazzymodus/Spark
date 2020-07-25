namespace InfiniteMinesweeper.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SparkEngine.Components;

    public struct MineTile : IComponent
    {
        public bool IsMine { get; }
        
        public byte Frame { get; set; }
    }
}
