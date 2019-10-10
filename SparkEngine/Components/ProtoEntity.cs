namespace SparkEngine.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SparkEngine.Rendering;

    public class ProtoEntity
    {
        //public ProtoEntity()
        //{

        //}

        public ProtoEntity(params Component[] components)
        {
            Components = components;
        }

        public Component[] Components { get; }

        public override bool Equals(object obj)
        {
            ProtoEntity protoEntity = obj as ProtoEntity;
            
            if (protoEntity == null || Components.Length != protoEntity.Components.Length)
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
            return base.GetHashCode();
        }
    }
}
