using SparkEngine.Components;

namespace SparkEngine.Entities
{
    public class ProtoEntity
    {
        private IComponent[] components;
        //public ProtoEntity()
        //{

        //}

        public ProtoEntity(params IComponent[] components)
        {
            this.components = components;
        }

        public ComponentBatch Batch => components;

        public ref IComponent[] GetComponentsByReference()
        {
            return ref components;
        }

        public void AddComponent<T>(T component) where T : struct, IComponent
        {
            var newLength = components.Length + 1;
            var oldArray = components;
            components = new IComponent[newLength];

            for (var i = 0; i < newLength - 1; i++) components[i] = oldArray[i];

            components[newLength - 1] = component;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ProtoEntity protoEntity) ||
                components.Length != protoEntity.components.Length) return false;

            for (var i = 0; i < components.Length; i++)
                if (!components[i].Equals(protoEntity.components[i]))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                for (var i = 0; i < components.Length; i++) hash = hash * 23 + components[i]?.GetHashCode() ?? 29;

                return hash;
            }
        }
    }
}