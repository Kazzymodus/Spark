using System.Collections.Generic;
using SparkEngine.Components;
using SparkEngine.States;
using SparkEngine.Systems.Tasks;

namespace SparkEngine.Systems
{
    public abstract class ComponentSystem
    {
        protected readonly List<int> pendingRemovals = new List<int>();

        public ComponentSystem(int maxTasks)
        {
            TaskManager = new TaskManager(null, maxTasks);
        }

        public bool IsUpdating { get; protected set; }

        public TaskManager TaskManager { get; protected set; }

        public void ScheduleTask(int task, int source, int target, UpdateInfo updateInfo)
        {
            TaskManager.ScheduleTask(task, source, target, updateInfo);
        }

        public abstract bool HasComponentOfEntity(int owner);

        protected internal abstract void Update(UpdateInfo updateInfo);
    }

    public abstract class ComponentSystem<T> : ComponentSystem where T : struct, IComponent
    {
        private readonly Dictionary<int, T> pendingAdds = new Dictionary<int, T>();

        public ComponentSystem(int maxSubs = GameState.MaxEntities, int maxTasks = 0)
            : base(maxTasks)
        {
            Subscribers = new ComponentPool<T>(maxSubs, GameState.MaxEntities);
        }

        protected ComponentPool<T> Subscribers { get; }

        //public ComponentSystem(int maxSubs = GameState.MaxEntities, Action<>)

        public T? GetComponent(int entity)
        {
            return Subscribers.GetComponent(entity);
        }

        public ref T GetComponentByReference(int entity)
        {
            return ref Subscribers.GetComponentByRef(entity);
        }

        public T[] GetComponents()
        {
            return Subscribers.GetComponentsCompact();
        }

        public sealed override bool HasComponentOfEntity(int entity)
        {
            return Subscribers.HasComponentOfEntity(entity);
        }

        public void AddNewComponentToEntity(T template, int entity, GameState state)
        {
            RegisterComponent(template, entity, state);
        }

        public virtual void OnAddComponent(ref T component, int owner, GameState gameState)
        {
        }

        public virtual void OnRemoveComponent(ref T component, int entity, GameState state)
        {
        }

        protected internal override void Update(UpdateInfo updateInfo)
        {
            IsUpdating = true;

            var components = Subscribers.Components;
            var skipList = Subscribers.AvailableIndices;

            for (var i = 0; i < Subscribers.NextIndex; i++)
            {
                if (skipList.Contains(i))
                {
                    skipList.Remove(i);
                    continue;
                }

                UpdateComponent(ref components[i], i, updateInfo);
            }

            TaskManager.ExecuteTasks();
            TaskManager.ResetTasks();

            IsUpdating = false;

            if (pendingAdds.Count > 0)
            {
                foreach (var pending in pendingAdds)
                    if (!Subscribers.HasComponentOfEntity(pending.Key))
                        AddComponent(pending.Value, pending.Key);

                pendingAdds.Clear();
            }

            if (pendingRemovals.Count > 0)
            {
                foreach (var pending in pendingRemovals)
                    if (Subscribers.HasComponentOfEntity(pending))
                        RemoveComponent(pending);

                pendingRemovals.Clear();
            }
        }

        protected internal abstract void UpdateComponent(ref T component, int index, UpdateInfo updateInfo);

        public virtual void RegisterComponent(T component, int owner, GameState state)
        {
            OnAddComponent(ref component, owner, state);
            if (IsUpdating)
                pendingAdds.Add(owner, component);
            else
                AddComponent(component, owner);
        }

        public virtual void DestroyComponent(T component, int owner, GameState state)
        {
            OnRemoveComponent(ref component, owner, state);
            if (IsUpdating)
                pendingRemovals.Add(owner);
            else
                RemoveComponent(owner);
        }

        private void AddComponent(T component, int owner)
        {
            Subscribers.Add(component, owner);
        }

        private void RemoveComponent(int owner)
        {
            Subscribers.Remove(owner);
        }
    }
}