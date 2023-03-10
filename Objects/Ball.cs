using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class Ball : TTFObject
    {
        private double _timeTracker;
        public Ball()
        {

        }

        public Ball(Ball previous)
        {

        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            position = new Vector2(graphics.PreferredBackBufferWidth / 2,
               graphics.PreferredBackBufferHeight / 2);
            animationFrame = 1;
            _timeTracker = 0.0;
            frameRows = 1;
            framesPerRow = 13;
        }

        public override void Update(GameTime gameTime)
        {
            _timeTracker += gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeTracker >= 0.1)
            {
                animationFrame += 1;
                _timeTracker = 0.0;
            }
            if (animationFrame < 1 || animationFrame > 13)
            {
                animationFrame = 1;
            }
            sourceRectangle = new Rectangle((texture.Width / 13) * (animationFrame - 1), 0, texture.Width / 13, texture.Height);
        }
    }
}
