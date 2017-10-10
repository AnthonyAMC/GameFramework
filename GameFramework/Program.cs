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
    class Program
    {
        /// <summary>
        /// Main entry point static method for the program.
        /// </summary>
        /// <param name="args">The input arguments from the command line, if any.</param>
        static void Main(string[] args)
        {
            // Create a new game instance.
            GameInternal game = new GameInternal();
            // Run the game!
            game.Run();
        }
    }
}
