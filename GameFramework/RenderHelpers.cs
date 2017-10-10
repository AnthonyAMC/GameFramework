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
    /// <summary>
    /// Rendering helpers
    /// </summary>
    class RenderHelpers
    {
        //static Vector3[] Positions;
        //static Vector2[] TexCoords;
        //static uint[] Indices;

        /// <summary>
        /// Generate Triangle object
        /// </summary>
        /// <returns>A Triangle Vertex Array/Buffer Object.</returns>
        public static int CreateTriangle()
        {
            Vector3[] Positions = new Vector3[]
            {
                new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0)
            };
            Vector2[] TexCoords = new Vector2[]
            {
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0)
            };
            return BufferObject(Positions, TexCoords);
        }

        /// <summary>
        /// Generate Quadrilateral.
        /// </summary>
        /// <returns>A Quadrilateral Vertex Array/Buffer Object.</returns>
        public static int CreateQuadrilateral()
        {
            Vector3[] Positions = new Vector3[]
            {
                new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0),
                new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Vector3(0, 1, 0)
            };
            Vector2[] TexCoords = new Vector2[]
            {
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0),
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1)
            };
            return BufferObject(Positions, TexCoords);
        }

        /// <summary>
        /// Generate Cuboid object
        /// </summary>
        /// <returns>A Cuboid Vertex Array/Buffer Object.</returns>
        public static int CreateCuboid()
        {
            Vector3[] Positions = new Vector3[]
            {
                new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),
                new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0),

                new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(0, 1, 1),
                new Vector3(1, 1, 1), new Vector3(1, 0, 1), new Vector3(0, 1, 1),

                new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0),
                new Vector3(0, 1, 1), new Vector3(0, 0, 1), new Vector3(0, 1, 0),

                new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(1, 1, 0),
                new Vector3(1, 1, 1), new Vector3(1, 0, 1), new Vector3(1, 1, 0),

                new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
                new Vector3(1, 0, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 0),

                new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 0),
                new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(1, 1, 0)
            };
            Vector2[] TexCoords = new Vector2[]
            {
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 0),
                new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 0)
            };
            return BufferObject(Positions, TexCoords);
        }

        /// <summary>
        /// Buffer an object.
        /// </summary>
        /// <param name="Positions">Array of object vertices.</param>
        /// <param name="TexCoords">Array of texture coordinates.</param>
        /// <returns>The Vertex Array/Buffer Object.</returns>
        public static int BufferObject(Vector3[] Positions, Vector2[] TexCoords)
        {
            uint[] Indices = new uint[Positions.Length];
            for (uint i = 0; i < Indices.Length; i++)
            {
                Indices[i] = i;
            }
            int pos = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, pos);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Positions.Length * Vector3.SizeInBytes), Positions, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int tcs = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tcs);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(TexCoords.Length * Vector2.SizeInBytes), TexCoords, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            int ind = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Indices.Length * sizeof(uint)), Indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            int Object = GL.GenVertexArray();
            GL.BindVertexArray(Object);
            GL.BindBuffer(BufferTarget.ArrayBuffer, pos);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, tcs);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ind);
            GL.BindVertexArray(0);
            return Object;
        }

        /// <summary>
        /// Compiles a VertexShader and FragmentShader to a usable shader program.
        /// </summary>
        /// <param name="VS">The input VertexShader code.</param>
        /// <param name="FS">The input FragmentShader code.</param>
        /// <returns>The internal OpenGL program ID.</returns>
        public static int CompileShader(string vs, string fs)
        {
            int VertexObject = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexObject, vs);
            GL.CompileShader(VertexObject);
            string VS_Info = GL.GetShaderInfoLog(VertexObject);
            GL.GetShader(VertexObject, ShaderParameter.CompileStatus, out int VS_Status);
            if (VS_Status != 1)
            {
                throw new Exception("Error creating VertexShader. Error status: " + VS_Status + ", info: " + VS_Info);
            }
            int FragmentObject = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentObject, fs);
            GL.CompileShader(FragmentObject);
            string FS_Info = GL.GetShaderInfoLog(FragmentObject);
            GL.GetShader(FragmentObject, ShaderParameter.CompileStatus, out int FS_Status);
            if (FS_Status != 1)
            {
                throw new Exception("Error creating FragmentShader. Error status: " + FS_Status + ", info: " + FS_Info);
            }
            int Program = GL.CreateProgram();
            GL.AttachShader(Program, FragmentObject);
            GL.AttachShader(Program, VertexObject);
            GL.LinkProgram(Program);
            string str = GL.GetProgramInfoLog(Program);
            if (str.Length != 0)
            {
                throw new Exception("Linked shader with message: '" + str + "'" + " -- FOR -- " + vs + " -- -- " + fs);
            }
            GL.DeleteShader(FragmentObject);
            GL.DeleteShader(VertexObject);
            return Program;
        }

        /// <summary>
        /// Converts a texture (input by file name) to a valid GL texture.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <returns>The GL object.</returns>
        public static int LoadTexture(string file)
        {
            int Texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            Bitmap bmp = new Bitmap(file);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            bmp.UnlockBits(bmp_data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRefToTexture);
            bmp.Dispose();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            return Texture;
        }

        /// <summary>
        /// Apply the projection matrix from the camera POV.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <returns>The GL object.</returns>
        public static void CameraView()
        {
            Quaternion Q = GameInternal.GetRotation();
            Vector3 forward = Vector3.Transform(-Vector3.UnitZ, Q);

            Matrix4 View = Matrix4.LookAt(GameInternal.Center, GameInternal.Center + forward, Vector3.UnitY);
            Matrix4 Perspective = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI * 0.5f, GameInternal.Window.Width / (float)GameInternal.Window.Height, 0.1f, 1000f);

            Matrix4 Projection = View * Perspective;

            GL.UniformMatrix4(1, false, ref Projection);

            GL.ClearBuffer(ClearBuffer.Color, 0, new float[] { 0, 0, 0, 1 });
            GL.ClearBuffer(ClearBuffer.Depth, 0, new float[] { 1 });
        }
    }
}
