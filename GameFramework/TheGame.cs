using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace GameFramework
{
    /// <summary>
    /// The primary coding area for your game.
    /// </summary>
    public class TheGame
    {
        /// <summary>
        /// The backend internal game system.
        /// </summary>
        public GameInternal Backend;

        /// <summary>
        /// Used to set the window title.
        /// </summary>
        public static String Title = "Hey there!";

        /// <summary>
        /// The backend internal game system.
        /// </summary>
        public static int TextureFinal;

        /// <summary>
        /// Generic cube model.
        /// </summary>
        public static int Cube;

        /// <summary>
        /// List of all entities.
        /// </summary>
        private static List<Entity> entities = new List<Entity>();

        /// <summary>
        /// List of all entities to remove.
        /// </summary>
        static List<Entity> RemoveEntities = new List<Entity>();

        /// <summary>
        /// List of all entities to add.
        /// </summary>
        static List<Entity> NewEntities = new List<Entity>();

        internal static List<Entity> Entities { get => entities; set => entities = value; }

        /// <summary>
        /// Load anything we need here.
        /// </summary>
        public void Load()
        {
            GameInternal.Window.Title = Title;
            TextureFinal = GameInternal.Tex_White;
            Cube = RenderHelpers.CreateCuboid();
            Random random = new Random();
            Entities = new List<Entity>();
            for (int i = 0; i < 500; i++)
            {
                Entities.Add(new Cuboid()
                {
                    Texture = TextureFinal,
                    Model = Cube,
                    //Vertices = Positions.Length,
                    //How to return positions.length from Cube = RenderHelpers.CreateCuboid();
                    Vertices = 36,
                    Velocity = new Vector3((float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1),
                    Position = new Vector3((float)random.NextDouble() * 100, (float)random.NextDouble() * 100, (float)random.NextDouble() * 100),
                    Scale = new Vector3((float)random.NextDouble() + 1, (float)random.NextDouble() + 1, (float)random.NextDouble() + 1)
                });
            }
        }

        /// <summary>
        /// Rendering logic here.
        /// </summary>
        public void Render()
        {
            RenderHelpers.CameraView();
            foreach (Entity ent in Entities)
            {
                // GL.UseProgram(Backend.Secondary_Shader);
                ent.Render();
            }
            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }


        /// <summary>
        /// Update logic here.
        /// </summary>
        /// <param name="delta"></param>
        public void Tick(float delta)
        {
            InputHelpers.ControlTick();
            foreach (Entity ent in Entities)
            {
                ent.Update(delta);
            }
            foreach (Entity ent in RemoveEntities)
            {
                Entities.Remove(ent);
            }
            RemoveEntities.Clear();
            Entities.AddRange(NewEntities);
            NewEntities.Clear();
        }
    }
}