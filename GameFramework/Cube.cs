using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace GameFramework
{
    class Cuboid : Entity
    {

        public Vector3 Velocity;

        public override void Update(float delta)
        {
            Position += Velocity * delta;
        }
    }
}
