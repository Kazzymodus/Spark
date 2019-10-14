namespace SparkEngine.Systems.Batching
{ 
    using System;
    using System.Linq;
    using SparkEngine.Components;

    public class ProtoEntity
    {
        //public ProtoEntity()
        //{

        //}

        public ProtoEntity(params Component[] components)
        {
            foreach (Component component in components)
            {
                if (!component.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEquatable<>)))
                {
                    throw new ArgumentException($"Can not add {component} to ProtoEntity because it does not implement IEquatable<T>");
                }
            }

            Components = components;
        }

        public Component[] Components { get; }

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
