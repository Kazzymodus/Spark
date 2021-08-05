using System;
using System.Collections;
using System.Collections.Generic;

namespace SparkEngine.Components
{
    public struct ComponentBatch : IEnumerable
    {
        public ComponentBatch(params IComponent[] components)
        {
            Components = components;
        }

        public static implicit operator ComponentBatch(IComponent[] components)
        {
            return new ComponentBatch(components);
        }

        public IComponent[] Components;

        public IComponent this[int i] => Components[i];

        public int Length => Components.Length;

        public void AddComponent<T>(T component) where T : struct, IComponent
        {
            var newLength = Components.Length + 1;
            var oldArray = Components;
            Components = new IComponent[newLength];

            for (var i = 0; i < newLength - 1; i++) Components[i] = oldArray[i];

            Components[newLength - 1] = component;
        }

        public bool Contains<T>() where T : struct, IComponent
        {
            return Contains(typeof(T));
        }

        public bool Contains(Type requiredComponent)
        {
            for (var i = 0; i < Components.Length; i++)
                if (Components[i].GetType() == requiredComponent)
                    return true;

            return false;
        }

        public bool ContainsOnly(Type[] requiredComponents)
        {
            if (Components.Length != requiredComponents.Length) return false;

            return ContainsAll(requiredComponents);
        }

        public bool ContainsAll(Type[] requiredComponents)
        {
            if (Components.Length < requiredComponents.Length) return false;

            for (var i = 0; i < requiredComponents.Length; i++)
            {
                var containsSpecificComponent = false;

                for (var j = 0; j < Components.Length; j++)
                    if (Components[j].GetType() == requiredComponents[i] ||
                        Components[j].GetType().IsSubclassOf(requiredComponents[i]))
                    {
                        containsSpecificComponent = true;
                        break;
                    }

                if (!containsSpecificComponent) return false;
            }

            return true;
        }

        public T GetComponent<T>() where T : IComponent
        {
            for (var i = 0; i < Components.Length; i++)
                if (Components[i] is T)
                    return (T) Components[i];

            return default;
        }

        public T[] GetComponentsSingleType<T>() where T : IComponent
        {
            var validComponents = new List<T>();

            for (var i = 0; i < Components.Length; i++)
                if (Components[i] is T)
                    validComponents.Add((T) Components[i]);

            return validComponents.ToArray();
        }

        public void GetComponentsMultiType<T1, T2>(out T1 component1, out T2 component2)
            where T1 : IComponent
            where T2 : IComponent
        {
            var components = GetComponentsInTypeOrder(typeof(T1), typeof(T2));
            component1 = (T1) components[0];
            component2 = (T2) components[1];
        }

        public void GetComponentsMultiType<T1, T2, T3>(out T1 component1, out T2 component2, out T3 component3)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            var components = GetComponentsInTypeOrder(typeof(T1), typeof(T2), typeof(T3));
            component1 = (T1) components[0];
            component2 = (T2) components[1];
            component3 = (T3) components[2];
        }

        public void GetComponentsMultiType<T1, T2, T3, T4>(out T1 component1, out T2 component2, out T3 component3,
            out T4 component4)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            var components = GetComponentsInTypeOrder(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            component1 = (T1) components[0];
            component2 = (T2) components[1];
            component3 = (T3) components[2];
            component4 = (T4) components[3];
        }

        public IComponent[] GetComponentsInTypeOrder(params Type[] componentTypes)
        {
            var returnComponents = new IComponent[componentTypes.Length];

            for (var i = 0; i < componentTypes.Length; i++)
            for (var j = 0; j < Components.Length; j++)
                if (componentTypes[i] == Components[j].GetType() ||
                    Components[j].GetType().IsSubclassOf(componentTypes[i]))
                {
                    returnComponents[i] = Components[j];
                    break;
                }

            return returnComponents;
        }

        public Type[] GetTypeOrder()
        {
            var types = new Type[Components.Length];

            for (var i = 0; i < types.Length; i++) types[i] = Components[i].GetType();

            return types;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Components.GetEnumerator();
        }
    }
}