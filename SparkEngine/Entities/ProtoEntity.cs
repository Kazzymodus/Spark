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
            Components = components;
        }

        public IComponent[] Components { get; private set; }

        public ComponentBatch Batch
        {
            get => Components;
        }

        public void AddComponent(IComponent component)
        {
            int newLength = Components.Length + 1;
            IComponent[] oldArray = Components;
            Components = new IComponent[newLength];

            for (int i = 0; i < newLength - 1; i++)
            {
                Components[i] = oldArray[i];
            }

            Components[newLength - 1] = component;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ProtoEntity protoEntity) || Components.Length != protoEntity.Components.Length)
            {
                return false;
            }

            for (int i = 0; i < Components.Length; i++)
            {
                if (!Components[i].Equals(protoEntity.Components[i]))
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

                for (int i = 0; i < Components.Length; i++)
                {
                    hash = hash * 23 + Components[i]?.GetHashCode() ?? 29;
                }

                return hash;
            }
        }
    }
}
