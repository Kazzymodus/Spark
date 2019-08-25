using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkEngine.Components
{
    public struct ComponentBatch
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

        public T[] GetComponents<T>() where T : Component
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
    }
}
