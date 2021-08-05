using System;
using System.Collections.Generic;
using SparkEngine.Components;

namespace SparkEngine.States
{
    public abstract class ComponentPool
    {
        protected readonly ushort[] entityTable;

        public ComponentPool(int maxComponents, int maxEntities)
        {
            if (maxComponents > maxEntities)
                throw new ArgumentException(
                    $"maxComponents ({maxComponents}) can not be higher than maxEntities ({maxEntities})");

            if (maxComponents > ushort.MaxValue || maxEntities > ushort.MaxValue)
                throw new ArgumentException(
                    $"Both maxComponents ({maxComponents}) and maxEntities ({maxEntities}) must be lower than {ushort.MaxValue}");

            entityTable = new ushort[maxComponents];
        }

        public int NextIndex { get; private set; }
        internal List<int> AvailableIndices { get; } = new List<int>();

        public int GetComponentIndexOfEntity(int entity)
        {
            for (var index = 0; index < NextIndex; index++)
                if (entityTable[index] == entity)
                    return index;

            return -1;
        }

        public bool HasComponentOfEntity(int entity)
        {
            return GetComponentIndexOfEntity(entity) != -1;
        }

        public int GetEntityOfComponentIndex(int index)
        {
            return entityTable[index];
        }

        protected int GetNewIndex()
        {
            int index;

            if (AvailableIndices.Count > 0)
            {
                index = AvailableIndices[0];
                AvailableIndices.RemoveAt(0);
            }
            else
            {
                index = NextIndex++;
            }

            return index;
        }

        protected void ClearIndex(int index)
        {
            if (index == NextIndex - 1)
            {
                NextIndex--;

                while (AvailableIndices.Contains(NextIndex - 1)) AvailableIndices.Remove(--NextIndex);
            }
            else
            {
                AvailableIndices.Add(index);
            }
        }
    }

    public class ComponentPool<T> : ComponentPool where T : struct, IComponent
    {
        public ComponentPool(int maxComponents, int maxEntities)
            : base(maxComponents, maxEntities)
        {
            Components = new T[maxComponents];
        }

        internal T[] Components { get; }

        public T? this[int entity] => GetComponent(entity);

        public ref T GetComponentByRef(int entity)
        {
            var index = GetComponentIndexOfEntity(entity);

            if (index == -1)
                throw new ArgumentException(
                    $"Entity {entity} does not have an associated component of type {typeof(T)}");

            return ref Components[index];
        }

        public T? GetComponent(int entity)
        {
            var index = GetComponentIndexOfEntity(entity);

            if (index == -1) return null;

            return Components[index];
        }

        public T[] GetComponentsCompact()
        {
            var length = NextIndex - AvailableIndices.Count;

            var compactArray = new T[length];
            var compactIndex = 0;

            var skipList = AvailableIndices;

            for (var i = 0; i < NextIndex; i++)
            {
                if (skipList.Contains(i))
                {
                    skipList.Remove(i);
                    continue;
                }

                compactArray[compactIndex] = Components[i];
                compactIndex++;
            }

            return compactArray;
        }

        public void Add(T component, int owner)
        {
            var index = GetNewIndex();
            Components[index] = component;
            entityTable[index] = (ushort) owner;
        }

        public void Remove(int entity)
        {
            int index;

            for (index = 0; index < NextIndex; index++)
                if (entityTable[index] == entity)
                {
                    Components[index] = default;
                    entityTable[index] = 0;
                    ClearIndex(index);
                    return;
                }

            Console.WriteLine($"{this} does not have a component belonging to entity {entity}.");
        }
    }
}