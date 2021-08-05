using SparkEngine.Components;

namespace InfiniteMinesweeper.Components
{
    public struct MineTile : IComponent
    {
        public bool IsMine { get; }

        public byte Frame { get; set; }
    }
}