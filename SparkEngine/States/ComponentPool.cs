namespace SparkEngine.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Components;

    public abstract class ComponentPool
    {
        protected readonly int[] indexTable;
        public int NextIndex { get; private set; }
        internal List<int> AvailableIndices { get; } = new List<int>();

        public ComponentPool(int maxComponents, int maxEntities)
        {
            if (maxComponents > maxEntities)
            {
                throw new ArgumentException($"maxComponents ({maxComponents}) can not be higher than maxEntities ({maxEntities})");
            }

            indexTable = new int[maxEntities];
        }

        protected int GetNewIndex()
        {
            int index = -1;

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
            }
            else
            {
                AvailableIndices.Add(index);
            }
        }
    }

    public class ComponentPool<T> : ComponentPool where T : struct, IComponent
    {
        private T[] components;

        public ComponentPool(int maxComponents, int maxEntities)
            : base(maxComponents, maxEntities)
        {
            components = new T[maxComponents];
        }

        public bool HasComponentOfEntity(int entity)
        {
            int index = indexTable[entity];

            return index < NextIndex && !AvailableIndices.Contains(entity);
        }

        public T? GetComponent(int entity)
        {
            int index =  indexTable[entity];

            return HasComponentOfEntity(entity) ? (T?)components[index] : null;
        }

        public T[] GetComponentsCompact()
        {
            int length = NextIndex - AvailableIndices.Count;

            T[] updateArray = new T[length];
            int j = 0;

            List<int> skipList = AvailableIndices;

            for (int i = 0; i < NextIndex; i++)
            {
                if (skipList.Contains(i))
                {
                    skipList.Remove(i);
                    continue;
                }

                updateArray[j] = components[i];
                j++;
            }

            return updateArray;
        }

        internal ref T[] GetComponentsByReference()
        {
            return ref components;
        }

        public T this[int entity]
        {
            get { return components[indexTable[entity]]; }
        }

        public void Add(T component, int owner)
        {
            int index = GetNewIndex();
            components[index] = component;
            indexTable[owner] = index;
        }

        public void Remove(int entity)
        {
            int index = indexTable[entity];

            components[index] = default(T);

            ClearIndex(index);
        }
    }
}
