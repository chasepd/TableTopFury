using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTopFury.Objects;

namespace TableTopFury
{
    internal static class GameState
    {
        private static List<Ball> _balls;
        private static List<Paddle> _paddles;
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
