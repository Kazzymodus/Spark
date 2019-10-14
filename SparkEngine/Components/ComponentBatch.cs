using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkEngine.Components
{
    public struct ComponentBatch : IEnumerable
    {
        public ComponentBatch(params Component[] components)
        {
            Components = components;
        }

        public static implicit operator ComponentBatch(Component[] components)
        {
            return new ComponentBatch(components);
        }

        public Component[] Components { get; }

        public Component this[int i]
        {
            get => Components[i];
        }

        public int Length
        {
            get => Components.Length;
        }

        public bool ContainsOnly(Type[] requiredComponents)
        {
            if (Components.Length != requiredComponents.Length)
            {
                return false;
            }

            return ContainsAll(requiredComponents);
        }

        public bool ContainsAll(Type[] requiredComponents)
        {
            if (Components.Length < requiredComponents.Length)
            {
                return false;
            }

            for (int i = 0; i < requiredComponents.Length; i++)
            {
                bool containsSpecificComponent = false;

                for (int j = 0; j < Components.Length; j++)
                {
                    if (Components[j].GetType() == requiredComponents[i] || Components[j].GetType().IsSubclassOf(requiredComponents[i]))
                    {
                        containsSpecificComponent = true;
                        break;
                    }
                }

                if (!containsSpecificComponent)
                {
                    return false;
                }
            }

            return true;
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < Components.Length; i++)
            {
                if (Components[i] is T)
                {
                    return (T)Components[i];
                }
            }

            return null;
        }

        public T[] GetComponentsSingleType<T>() where T : Component
        {
            List<T> validComponents = new List<T>();

            for (int i = 0; i < Components.Length; i++)
            {
                if (Components[i] is T)
                {
                    validComponents.Add((T)Components[i]);
                }
            }

            return validComponents.ToArray();
        }

        public void GetComponentsMultiType<T1, T2>(out T1 component1, out T2 component2)
            where T1 : Component
            where T2 : Component
        {
            Component[] components = GetComponentsInTypeOrder(typeof(T1), typeof(T2));
            component1 = (T1)components[0];
            component2 = (T2)components[1];
        }

        public void GetComponentsMultiType<T1, T2, T3>(out T1 component1, out T2 component2, out T3 component3)
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            Component[] components = GetComponentsInTypeOrder(typeof(T1), typeof(T2), typeof(T3));
            component1 = (T1)components[0];
            component2 = (T2)components[1];
            component3 = (T3)components[2];
        }

        public void GetComponentsMultiType<T1, T2, T3, T4>(out T1 component1, out T2 component2, out T3 component3, out T4 component4)
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            Component[] components = GetComponentsInTypeOrder(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            component1 = (T1)components[0];
            component2 = (T2)components[1];
            component3 = (T3)components[2];
            component4 = (T4)components[3];
        }

        public Component[] GetComponentsInTypeOrder(params Type[] componentTypes)
        {
            Component[] returnComponents = new Component[componentTypes.Length];

            for (int i = 0; i < componentTypes.Length; i++)
            {
                for (int j = 0; j < Components.Length; j++)
                {
                    if (componentTypes[i] == Components[j].GetType() || Components[j].GetType().IsSubclassOf(componentTypes[i]))
                    {
                        returnComponents[i] = Components[j];
                        break;
                    }
                }
            }

            return returnComponents;
        }

        public Type[] GetTypeOrder()
        {
            Type[] types = new Type[Components.Length];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = Components[i].GetType();
            }

            return types;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Components.GetEnumerator();
        }
    }
}
