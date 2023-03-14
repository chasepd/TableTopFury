using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private int _preferredBackBufferWidth;
        private int _preferredBackBufferHeight;
        protected bool isExploding;
        const float scaleModifier = 1f;
        const int ballSizeModifier = (int)(scaleModifier * 7);
        protected ContentManager _contentManager;
        public Texture2D explosionTexture;
        public Texture2D ballTexture;
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
            if (new Random().Next(1, 3) == 2)
            {
                speedX *= -1;
            }
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
            position = new Vector2((graphics.PreferredBackBufferWidth / 2) + ballSizeModifier,
               graphics.PreferredBackBufferHeight / 2 + ballSizeModifier); 
            
            _preferredBackBufferHeight = graphics.PreferredBackBufferHeight;
            _preferredBackBufferWidth = graphics.PreferredBackBufferWidth;

        }        

        public void Explode()
        {
            texture = explosionTexture;
            isExploding = true;
            rotation = 0;
            framesPerRow = 14;
        }

        public override void LoadContent(ContentManager content)
        {
            _contentManager = content;
        }

        private Rectangle GetCollisionBoundaries()
        {
            if (position.X < _preferredBackBufferWidth / 2)
            {
                return new Rectangle((int)position.X - ballSizeModifier + (3 * (int)scaleModifier), (int)position.Y - ballSizeModifier, ballSizeModifier, ballSizeModifier);
            }
            else
            {
                return new Rectangle((int)position.X - ballSizeModifier - (3 * (int)scaleModifier), (int)position.Y - ballSizeModifier, ballSizeModifier, ballSizeModifier);
            }
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
                if (animationFrame == 14) 
                {                   
                    isExploding = false;
                    texture = ballTexture;
                    framesPerRow = 13;
                }
            }
            else
            {
                if (position.X > graphics.PreferredBackBufferWidth - ballSizeModifier)
                {
                    Explode();
                }
                if (position.X < ballSizeModifier)
                {
                    Explode();         
                }

                if (position.Y > graphics.PreferredBackBufferHeight - ballSizeModifier)
                {
                    position.Y = graphics.PreferredBackBufferHeight - ballSizeModifier;
                    speedY += new Random().Next(-1, 1);
                    speedY *= -1;
                }
                if (position.Y < ballSizeModifier)
                {
                    position.Y = ballSizeModifier;
                    speedY += new Random().Next(-1, 1);
                    speedY *= -1;
                }

                foreach (TTFObject obj in objects)
                {
                    if (obj != this)
                    {
                        if (obj.IsCollisionPoint(GetCollisionBoundaries()))
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
                    rotation += speedX + speedY;
                    _rotationTimeTracker = 0;
                }
                
            }

            if (rotation >= 360)
            {
                rotation = 0;
            }
            sourceRectangle = new Rectangle((texture.Width / framesPerRow) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            if (animationFrame < framesPerRow || isExploding)
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
                speedX = 5;
                speedY = 1;
                if (new Random().Next(1, 3) == 2)
                {
                    speedX *= -1;
                }
            }
        }

        public override bool IsCollisionPoint(Rectangle other)
        {          
            return GetCollisionBoundaries().Intersects(other);
        }
    }
}
