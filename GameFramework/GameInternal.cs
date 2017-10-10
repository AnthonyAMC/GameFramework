using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace GameFramework
{
    /// <summary>
    /// The internal game code (instantiated only from Program.cs).
    /// </summary>
    public class GameInternal
    {
        /// <summary>
        /// The primary window for the game.
        /// Created within Run().
        /// </summary>
        public static GameWindow Window;

        /// <summary>
        /// The game this internal is connected to.
        /// </summary>
        public TheGame Game;

        /// <summary>
        /// Used to set the window title.
        /// Set in TheGame.Title
        /// </summary>
        public String Title;

        /// <summary>
        /// The primary shader used by the game.
        /// </summary>
        public int Primary_Shader;

        /// <summary>
        /// The secondary shader used by the game.
        /// </summary>
        public int Secondary_Shader;

        /// <summary>
        /// Plain white texture.
        /// </summary>
        public static int Tex_White;

        /// <summary>
        /// Location of the Camera.
        /// </summary>
        public static Vector3 Center = Vector3.UnitZ * 100;

        /// <summary>
        /// Camera angle.
        /// </summary>
        public static float Angle = 0;

        /// <summary>
        /// Camera pitch.
        /// </summary>
        public static float Pitch = 0;

        /// <summary>
        /// Delta time.
        /// How much time has elapsed since the previous event.
        /// </summary>
        public static float Delta;

        /// <summary>
        /// 89 Degrees in Radians.
        /// </summary>
        public static readonly float Degrees89 = MathHelper.DegreesToRadians(89);

        public static Quaternion GetRotation()
        {
            return Quaternion.FromEulerAngles(0, Angle, Pitch);
        }

        /// <summary>
        /// Entry point to run the game (called by Program.cs).
        /// </summary>
        public void Run()
        {
            Window = new GameWindow(800, 600, GraphicsMode.Default, Title,
                GameWindowFlags.Default, DisplayDevice.Default,
                4, 3, GraphicsContextFlags.ForwardCompatible);
            Window.Load += Window_Load;
            Window.Resize += Window_Resize;
            Window.RenderFrame += Window_RenderFrame;
            Window.KeyDown += InputHelpers.Keydown;
            Window.KeyUp += InputHelpers.Keyup;
            Window.VSync = VSyncMode.On;
            // Reduce cpu waste
            Window.ReduceCPUWaste = true;
            Window.Location = Point.Empty;
            Window.Run();
        }

        /// <summary>
        /// Fired automatically when the window is run and is loading.
        /// Used to load data.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Empty event arguments slot.</param>
        private void Window_Load(object sender, EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            Primary_Shader = RenderHelpers.CompileShader(File.ReadAllText("Shader2.vs"), File.ReadAllText("Shader2.fs"));
            Secondary_Shader = RenderHelpers.CompileShader(File.ReadAllText("Shader1.vs"), File.ReadAllText("Shader1.fs"));
            Tex_White = RenderHelpers.LoadTexture("white.png");
            Game = new TheGame()
            {
                Backend = this
            };
            Game.Load();
        }

        /// <summary>
        /// Fired when is being resized.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Empty event arguments slot.</param>
        private static void Window_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Window.Width, Window.Height);
        }

        /// <summary>
        /// Fired automatically whenever the window is rendering any singular frame.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Event arguments related to the rendering.</param>
        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            Delta = (float)e.Time;
            GL.ClearBuffer(ClearBuffer.Color, 0, new float[] { 0, 0, 0, 1 });
            GL.UseProgram(Secondary_Shader);
            GL.Uniform4(3, Color4.White);
            GL.BindTexture(TextureTarget.Texture2D, Tex_White);
            GL.BindVertexArray(0);
            Game.Render();
            // Always check for errors!
            //CheckError("Render Complete");
            Game.Tick(Delta);
            // Always check for errors!
            //CheckError("Tick Complete");
            Window.SwapBuffers();
        }

        /*
        /// <summary>
        /// Checks for errors within the graphics engine.
        /// </summary>
        /// <param name="time">Indicates when or where there might be an error.</param>
        public static void CheckError(string time)
        {
            // Read the GL error code.
            ErrorCode ec = GL.GetError();
            // So long as the error code isn't "none":
            while (ec != ErrorCode.NoError)
            {
                // Output some information about it!
                Console.WriteLine("Error: " + ec + ", at: " + time + "::\n " + Environment.StackTrace);
                // Reset the error code to whatever's next in line.
                ec = GL.GetError();
            }
        }
        */
    }
}