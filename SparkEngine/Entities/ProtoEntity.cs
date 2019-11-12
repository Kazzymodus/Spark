namespace SparkEngine.Entities
{ 
    using System;
    using System.Linq;
    using SparkEngine.Components;

    public class ProtoEntity
    {
        //public ProtoEntity()
        //{

        //}

        public ProtoEntity(params IComponent[] components)
        {
            this.components = components;
        }

        private IComponent[] components;

        public ComponentBatch Batch
        {
            get => components;
        }

        public ref IComponent[] GetComponentsByReference()
        {
            return ref components;
        }

        public void AddComponent<T>(T component) where T : struct, IComponent
        {
            int newLength = components.Length + 1;
            IComponent[] oldArray = components;
            components = new IComponent[newLength];

            for (int i = 0; i < newLength - 1; i++)
            {
                components[i] = oldArray[i];
            }

            components[newLength - 1] = component;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ProtoEntity protoEntity) || components.Length != protoEntity.components.Length)
            {
                return false;
            }

            for (int i = 0; i < components.Length; i++)
            {
                if (!components[i].Equals(protoEntity.components[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                for (int i = 0; i < components.Length; i++)
                {
                    hash = hash * 23 + components[i]?.GetHashCode() ?? 29;
                }

                return hash;
            }
        }
    }
}
