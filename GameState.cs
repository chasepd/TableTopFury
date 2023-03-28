using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Modes;
using TableTopFury.Objects;

namespace TableTopFury
{
    internal static class GameState
    {
        private static List<Ball> _balls;
        private static List<Paddle> _paddles;
        public static ContentManager Content;
        public static GraphicsDeviceManager Graphics;
        public static GameTime GameTime;
        public static SpriteBatch CurrentSpriteBatch;
        public static Mode CurrentMode { get; set; }
        public static Board Board { get; set; }
        public static List<Ball> Balls
        {
            get
            {
                if (_balls == null)
                {
                    _balls = new List<Ball>();
                }
                return _balls;
            }
        }
        public static List<Paddle> Paddles
        {
            get
            {
                if (_paddles == null)
                {
                    _paddles = new List<Paddle>();
                }
                return _paddles;
            }
        }
    }
}
