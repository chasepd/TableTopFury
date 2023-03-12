using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class Ball : TTFObject
    {
        private double _explosionTimeTracker;
        private double _rotationTimeTracker;
        protected bool isExploding;
        const float scaleModifier = 2f;
        const int ballSizeModifier = (int)(scaleModifier * 8);
        public Ball() 
        {
            animationFrame = 1;
            _explosionTimeTracker = 0.0;
            _rotationTimeTracker = 0.0;
            frameRows = 1;
            framesPerRow = 13;
            isExploding = false;
            rotation = 0;
            speedX = 5;
            speedY = 1;
        }

        public Ball(Ball previous)
        {
            position = previous.position;
            animationFrame = 1;
            _explosionTimeTracker = 0.0;
            _rotationTimeTracker = 0.0;
            frameRows = 1;
            framesPerRow = 13;
            isExploding = false;
            rotation = previous.rotation;
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            position = new Vector2(graphics.PreferredBackBufferWidth / 2,
               graphics.PreferredBackBufferHeight / 2);            
        }

        public void Explode()
        {
            isExploding = true;
            rotation = 0;
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            if (isExploding)
            {
                _rotationTimeTracker = 0.0;
                _explosionTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;

                if (_explosionTimeTracker >= 0.1)
                {
                    animationFrame += 1;
                    _explosionTimeTracker = 0.0;
                }
                if (animationFrame == 13) 
                {                   
                    isExploding = false;
                }
            }
            else
            {
                if (position.X > graphics.PreferredBackBufferWidth - ballSizeModifier)
                {
                    position.X = graphics.PreferredBackBufferWidth - ballSizeModifier;
                    speedX += new Random().Next(-1, 1);
                    speedY += new Random().Next(-1, 1);
                    speedX *= -1;
                }
                if (position.X < ballSizeModifier)
                {
                    Explode();
                    //position.X = ballSizeModifier;
                    //speedX += new Random().Next(-1, 1);
                    //speedY += new Random().Next(-1, 1);
                    //speedX *= -1;                 
                }

                if (position.Y > graphics.PreferredBackBufferHeight - ballSizeModifier)
                {
                    position.Y = graphics.PreferredBackBufferHeight - ballSizeModifier;
                    speedX += new Random().Next(-1, 1);
                    speedY += new Random().Next(-1, 1);
                    speedY *= -1;
                }
                if (position.Y < ballSizeModifier)
                {
                    position.Y = ballSizeModifier;
                    speedX += new Random().Next(-1, 1);
                    speedY += new Random().Next(-1, 1);
                    speedY *= -1;
                }

                foreach (TTFObject obj in objects)
                {
                    if (obj != this)
                    {
                        if (obj.IsCollisionPoint(new Vector2(position.X + ballSizeModifier, position.Y + ballSizeModifier), new Vector2(ballSizeModifier * 2, ballSizeModifier * 2)))
                        {
                            speedX += new Random().Next(-1, 1);
                            speedY += new Random().Next(-1, 1);
                            speedX *= -1;
                        }
                    }
                }

                position.Y += speedY;
                position.X += speedX;

                _rotationTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
                if (_rotationTimeTracker >= 0.05 && !isExploding)
                {
                    rotation += speedX;
                    _rotationTimeTracker = 0;
                }
                
            }

            if (rotation >= 360)
            {
                rotation = 0;
            }
            sourceRectangle = new Rectangle((texture.Width / 13) * (animationFrame - 1), 0, texture.Width / 13, texture.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            if (animationFrame < 13 || isExploding)
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
            else
            {
                animationFrame = 1;
                position = new Vector2(graphics.PreferredBackBufferWidth / 2,
                 graphics.PreferredBackBufferHeight / 2);
            }
        }

        public override bool IsCollisionPoint(Vector2 point, Vector2 size)
        {
            throw new NotImplementedException();
        }
    }
}
