using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace GameFramework
{
    class GameHelpers
    {

        /// <summary>
        /// Listens to mouse movement.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Event data.</param>
        public void Window_MouseMove(object sender, MouseMoveEventArgs e)
        {
            MouseCoords = new Vector2(e.X, e.Y);
        }

        /// <summary>
        /// Stores current mouse coordinates.
        /// </summary>
        public Vector2 MouseCoords = Vector2.Zero;
    }
}
