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
    abstract class Entity
    {
        public Vector3 Position;

        public int Texture;

        public int Model;

        public int Vertices;

        public Vector3 Scale;

        public abstract void Update(float delta);

        public void Render()
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            Matrix4 ModelMatrix = Matrix4.CreateScale(Scale.X, Scale.Y, Scale.Z) * Matrix4.CreateTranslation(
                Position.X, Position.Y, Position.Z);
            GL.UniformMatrix4(2, false, ref ModelMatrix);
            GL.BindVertexArray(Model);
            GL.DrawElements(PrimitiveType.Triangles, Vertices, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}
