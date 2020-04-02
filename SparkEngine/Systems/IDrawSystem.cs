using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SparkEngine.Components;
using SparkEngine.Rendering;
using SparkEngine.States;
using SparkEngine.Utilities;

namespace SparkEngine.Systems
{
    public interface IDrawSystem
    {
        void Draw(DrawInfo drawInfo);
    }
}
