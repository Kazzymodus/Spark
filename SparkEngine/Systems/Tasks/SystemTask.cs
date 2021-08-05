namespace SparkEngine.Systems.Tasks
{
    public struct SystemTask
    {
        public readonly byte task;
        public readonly ushort source;
        public readonly ushort target;
        public readonly UpdateInfo updateInfo;

        public SystemTask(int task, int source, int target, UpdateInfo updateInfo)
        {
            this.task = (byte) task;
            this.source = (ushort) source;
            this.target = (ushort) target;
            this.updateInfo = updateInfo;
        }
    }
}