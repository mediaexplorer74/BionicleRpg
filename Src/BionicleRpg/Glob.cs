using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;


namespace GameManager
{

    public static class Glob
    {
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static float TotalGameTime { get; set; }
        public static GameTime GameTime { get; set; } = new GameTime();

        public static float DeltaTime { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
       
        public static Point WindowSize { get; set; }

        public static void Update(GameTime gt)
        {
            double ts = gt.ElapsedGameTime.TotalSeconds;
       
            DeltaTime = (float)gt.ElapsedGameTime.TotalSeconds;

        }            

    }
}
