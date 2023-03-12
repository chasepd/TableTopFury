using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal class PlayerPaddle : Paddle
    {
        const float scaleModifier = 3f;
        private double _animateTimeTracker;
        private int absoluteSpeed = 10;        
        const int paddleHeightModifier = (int)(scaleModifier * 24);
        const int paddleWidthModifier = (int)(scaleModifier * 8);
        public PlayerPaddle() : base() 
        {
            animationFrame = 1;
            frameRows = 1;
            framesPerRow = 7;
            _animateTimeTracker = 0.0;
            speedX = 0;
            speedY = 0;
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            position = new Vector2(0 + paddleWidthModifier, graphics.PreferredBackBufferHeight / 2);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("BasicPaddle");
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            _animateTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.W))
            {
                speedY = -1 * absoluteSpeed;
            }
            else if (kstate.IsKeyDown(Keys.S))
            {
                speedY = absoluteSpeed; 
            }
            else
            {
                speedY = 0;
            }

            position.Y += speedY;
            position.X += speedX;

            if (position.Y < 0 + paddleHeightModifier)
            {
                position.Y = 0 + paddleHeightModifier;
            }

            if (position.Y > graphics.PreferredBackBufferHeight - paddleHeightModifier)
            {
                position.Y = graphics.PreferredBackBufferHeight - paddleHeightModifier;
            }

            if (_animateTimeTracker > .4)
            {
                animationFrame++;
                _animateTimeTracker = 0.0;
            }

            if(animationFrame >= framesPerRow)
            {
                animationFrame = 1;
            }
            sourceRectangle = new Rectangle((texture.Width / framesPerRow) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(
                 texture,
                 position,
                 sourceRectangle,
                 Color.White,
                 MathHelper.ToRadians(rotation),
                 new Vector2(texture.Width / (framesPerRow * 2), texture.Height / (frameRows * 2)),
                 scaleModifier,
                 SpriteEffects.None,
                 0f
                );
        }

        public override bool IsCollisionPoint(Vector2 point)
        {
            throw new NotImplementedException();
        }
    }
}
