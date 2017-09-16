using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
        public GameWindow Window;

        /// <summary>
        /// Entry point to run the game (called by Program.cs).
        /// </summary>
        public void Run()
        {
            // Construct a new window in 'game' format.
            // Give it: 800x600 resolution,
            // Standard-rate graphics for Windows,
            // Titled "wow!",
            // Fixed (non-resizable) window,
            // On the default display device (EG for multiple monitors),
            // OpenGL 4.3 (GLSL 430),
            // Forward-Compatibility mode of OpenGL (no backwards support!)
            Window = new GameWindow(800, 600, GraphicsMode.Default, "Wow!",
                GameWindowFlags.FixedWindow, DisplayDevice.Default,
                4, 3, GraphicsContextFlags.ForwardCompatible);
            // Add event when the window loads
            Window.Load += Window_Load;
            Window.Resize += Window_Resize;
            // Add event when the window is rendering any singular frame.
            Window.RenderFrame += Window_RenderFrame;
            Window.ReduceCPUWaste = true;
            Window.Location = Point.Empty;
            // Enable Vertical Sync, meaning the game will only run as fast as the monitor updates
            // Preventing 'useless render cycles'
            Window.VSync = VSyncMode.On;
            // Run the game-window.
            Window.Run();
        }

        /// <summary>
        /// The primary shader used by the game.
        /// </summary>
        public int Primary_Shader;

        /// <summary>
        /// Fired automatically when the window is run and is loading.
        /// Used to load data.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Empty event arguments slot.</param>
        private void Window_Load(object sender, EventArgs e)
        {
            // Read the text of the vertex shader from file.
            string VS = File.ReadAllText("Shader_VS.glsl");
            // Read the text of the fragment shader from file.
            string FS = File.ReadAllText("shader_FS.glsl");
            // Compile the primary shader program.
            Primary_Shader = RenderHelpers.CompileShader(VS, FS);
            // Load some default textures
            Tex_White = RenderHelpers.LoadTexture("white.png");
            Tex_Red_X = RenderHelpers.LoadTexture("red_x.png");
            // Load a VBO
            VBO_Cuboid = RenderHelpers.CreateCuboid();
            // Construct YOUR GAME!
            Game = new TheGame()
            {
                // Configure the game backend to be this GameInternal object.
                Backend = this
            };
            // Load your game!
            Game.Load();
        }

        /// <summary>
        /// Fired when is being resized.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Empty event arguments slot.</param>
        private void Window_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Window.Width, Window.Height);
        }

        /// <summary>
        /// A BOX vertex array/buffer object.
        /// </summary>
        public int VBO_Cuboid;

        /// <summary>
        /// Plain white texture.
        /// </summary>
        public int Tex_White;

        /// <summary>
        /// Red X as a texture.
        /// </summary>
        public int Tex_Red_X;

        /// <summary>
        /// The game this internal is connected to.
        /// </summary>
        public TheGame Game;

        /// <summary>
        /// Fired automatically whenever the window is rendering any singular frame.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Event arguments related to the rendering.</param>
        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            // Clear the back buffer for fresh drawing!
            GL.ClearBuffer(ClearBuffer.Color, 0, new float[] { 0, 0, 0, 1 });
            // Bind the primary shader.
            GL.UseProgram(Primary_Shader);
            // Default color of white
            GL.Uniform4(3, Color4.White);
            // Prevent texture randomness by ensuring plain white is bound by default.
            GL.BindTexture(TextureTarget.Texture2D, Tex_White);
            // Default to no VBO.
            GL.BindVertexArray(0);
            // Render your game!
            Game.Render();
            // Always check for errors!
            RenderHelpers.CheckError("Render Complete");
            // Tick your game, while the rendering logic is processing on the GPU!
            Game.Tick(e.Time);
            // Always check for errors!
            RenderHelpers.CheckError("Tick Complete");
            // We rendered everything to the graphics card...
            // now present it to the monitor screen!
            Window.SwapBuffers();
        }
    }
}
