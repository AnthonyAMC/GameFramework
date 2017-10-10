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
    class InputHelpers
    {

        public static Dictionary<Key, Vector3> KeyboardMap = new Dictionary<Key, Vector3>()
        {
            {Key.S, Vector3.UnitZ},
            {Key.W, -Vector3.UnitZ},
            {Key.D, Vector3.UnitX},
            {Key.A, -Vector3.UnitX},
            {Key.Space, Vector3.UnitY},
            {Key.LShift, -Vector3.UnitY},
        };

        /// <summary>
        /// List holding all active keyboard keys.
        /// </summary>
        public static List<Key> ActiveKeys = new List<Key>();

        /// <summary>
        /// Current Vector modifier.
        /// </summary>
        public static Vector3 CurrentVector = Vector3.Zero;

        /// <summary>
        /// Sets the current vector modifier.
        /// </summary>
        public static void SetCurrentVector()
        {
            CurrentVector = Vector3.Zero;
            for (int i = 0; i < ActiveKeys.Count; i++)
            {
                if (KeyboardMap.TryGetValue(ActiveKeys[i], out Vector3 v))
                {
                    CurrentVector += v;
                }
            }
            /*
            foreach (Vector3 vec in ActiveKeys.Intersect(KeyboardMap.Keys).Select(k => KeyboardMap[k]))
            {
                CurrentVector += vec;
            }
            */
        }

        /// <summary>
        /// Listens for KeyDown event.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Event data.</param>
        public static void Keydown(object sender, KeyboardKeyEventArgs e)
        {
            Key currentKey = e.Key;
            if (!ActiveKeys.Contains(currentKey))
            {
                ActiveKeys.Add(currentKey);
                if (KeyboardMap.ContainsKey(currentKey))
                {
                    SetCurrentVector();
                }
            }
        }

        /// <summary>
        /// Listens for KeyUp event.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Event data.</param>
        public static void Keyup(object sender, KeyboardKeyEventArgs e)
        {
            Key currentKey = e.Key;
            if (ActiveKeys.Contains(currentKey))
            {
                ActiveKeys.Remove(currentKey);
                if (KeyboardMap.ContainsKey(currentKey))
                {
                    SetCurrentVector();
                }
            }
        }

        /// <summary>
        /// Listens to mouse movement.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseMove(object sender, MouseMoveEventArgs e)
        {
            MouseCoords = new Vector2(e.X, e.Y);
        }

        /// <summary>
        /// Stores current mouse coordinates.
        /// </summary>
        public static Vector2 MouseCoords = Vector2.Zero;


        /// <summary>
        /// Tick operation for control inputs.
        /// </summary>
        public static void ControlTick()
        {
            MouseState ms = Mouse.GetState();
            if (ms.IsButtonDown(MouseButton.Left))
            {
                GameInternal.Angle -= (ms.X - MouseCoords.X) * 0.005f;
                //GameInternal.Angle = (GameInternal.Angle < 0f) ? 360f : (GameInternal.Angle > 360f) ? 0f : GameInternal.Angle;
                GameInternal.Pitch -= (ms.Y - MouseCoords.Y) * 0.005f;
                GameInternal.Pitch = (GameInternal.Pitch < -GameInternal.Degrees89) ? -GameInternal.Degrees89 : (GameInternal.Pitch > GameInternal.Degrees89) ? GameInternal.Degrees89 : GameInternal.Pitch;
            }
            MouseCoords = new Vector2(ms.X, ms.Y);
            Quaternion Q = GameInternal.GetRotation();
            if (InputHelpers.CurrentVector != Vector3.Zero)
            {
                GameInternal.Center += Vector3.Transform(InputHelpers.CurrentVector, Q);
            }
        }
    }
}
