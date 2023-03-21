using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private double _ballAnimateTimeTracker;
        private double _rotationTimeTracker;
        private int _preferredBackBufferWidth;
        private int _preferredBackBufferHeight;
        public bool isExploding;
        float scaleModifier;
        const int explodeFrames = 14;
        const int ballFrames = 4;
        protected ContentManager _contentManager;
        public Texture2D explosionTexture;
        public Texture2D ballTexture;
        protected SoundEffect _explosionSound;
        protected List<SoundEffect> _contactSounds;
        public Ball() 
        {
            animationFrame = 1;
            _explosionTimeTracker = 0.0;
            _rotationTimeTracker = 0.0;
            frameRows = 1;
            framesPerRow = ballFrames;
            isExploding = false;
            rotation = 0;
            speedX = 5;
            speedX += new Random().Next(0, 10);
            if (new Random().Next(1, 3) == 2)
            {
                speedX *= -1;
            }
            speedY = new Random().Next(-1, 2);
            _contactSounds = new List<SoundEffect>();
        }

        public Ball(Ball previous)
        {
            position = previous.position;
            animationFrame = 1;
            _explosionTimeTracker = 0.0;
            _rotationTimeTracker = 0.0;
            frameRows = 1;
            framesPerRow = ballFrames;
            isExploding = false;
            rotation = previous.rotation;
            _contactSounds = new List<SoundEffect>();
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            position = new Vector2(graphics.PreferredBackBufferWidth / 2 - ((texture.Width / framesPerRow) / 2),
               graphics.PreferredBackBufferHeight / 2 - ((texture.Height / frameRows) / 2));

            _preferredBackBufferHeight = graphics.PreferredBackBufferHeight;
            _preferredBackBufferWidth = graphics.PreferredBackBufferWidth;

            scaleModifier = graphics.PreferredBackBufferHeight / 480;

        }        

        public void Explode()
        {
            texture = explosionTexture;
            isExploding = true;
            rotation = 0;
            framesPerRow = explodeFrames;
            _explosionSound.CreateInstance().Play();
        }

        private int GetLeftEdge(GraphicsDeviceManager graphics)
        {
            if (graphics.PreferredBackBufferWidth * 9 > graphics.PreferredBackBufferHeight * 16)
            {
                return (graphics.PreferredBackBufferWidth - (16 * graphics.PreferredBackBufferHeight) / 9) / 2;
            }
            else
            {
                return (int)(graphics.PreferredBackBufferWidth - (texture.Width * scaleModifier / framesPerRow / 2));
            }
        }

        private int GetRightEdge(GraphicsDeviceManager graphics)
        {
            if (graphics.PreferredBackBufferWidth * 9 > graphics.PreferredBackBufferHeight * 16)
            {
                return graphics.PreferredBackBufferWidth - (graphics.PreferredBackBufferWidth - (16 * graphics.PreferredBackBufferHeight) / 9) / 2;
            }
            else
            {
                return (int)(graphics.PreferredBackBufferWidth - (texture.Width * scaleModifier / framesPerRow / 2));
            }
        }

        public override void LoadContent(ContentManager content)
        {
            _contentManager = content;
        }

        private Rectangle GetCollisionBoundaries()
        {
            //Position appears to be returning center. Remove half the ball size to get the correct coordinates for the top left of the collision rectangle.
            return new Rectangle((int)position.X - (int)(texture.Width / framesPerRow / 2 * scaleModifier), 
                (int)position.Y - (int)(texture.Height / frameRows / 2 * scaleModifier), 
                (int)(texture.Width / framesPerRow * scaleModifier), 
                (int)(texture.Height / frameRows * scaleModifier));            
            
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            if (isExploding)
            {
                _rotationTimeTracker = 0.0;
                _ballAnimateTimeTracker = 0.0;
                _explosionTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;

                if (_explosionTimeTracker >= 0.1)
                {
                    animationFrame += 1;
                    _explosionTimeTracker = 0.0;
                }
                if (animationFrame > explodeFrames) 
                {                   
                    isExploding = false;
                    texture = ballTexture;
                    framesPerRow = ballFrames;
                    position = new Vector2(_preferredBackBufferWidth/2, _preferredBackBufferHeight/2);
                }
            }
            else
            {
                _ballAnimateTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
                if (_ballAnimateTimeTracker >= new Random().NextDouble() + 0.3)
                {
                    animationFrame = new Random().Next(1, ballFrames + 1);
                    _ballAnimateTimeTracker = 0.0;
                } 

                if (position.X > GetRightEdge(graphics))
                {
                    Explode();
                }
                if (position.X < GetLeftEdge(graphics))
                {
                    Explode();         
                }

                if (position.Y > graphics.PreferredBackBufferHeight - ((texture.Height / frameRows) / 2))
                {
                    position.Y = graphics.PreferredBackBufferHeight - ((texture.Height / frameRows) / 2);
                    speedY += new Random().Next(-1, 1);
                    if (speedY > 0)
                    {
                        speedY *= -1;
                    }
                    else
                    {
                        speedY = -1;
                    }
                }
                if (position.Y < ((texture.Height / frameRows) / 2))
                {
                    position.Y = ((texture.Height / frameRows) / 2);
                    speedY += new Random().Next(-1, 1);
                    if (speedY < 0)
                    {
                        speedY *= -1;
                    }
                    else
                    {
                        speedY = 1;
                    }
                }

                foreach (TTFObject obj in objects)
                {
                    if (obj is Paddle)
                    {
                        int collisionResult = obj.IsCollisionPoint(GetCollisionBoundaries());
                        if (collisionResult == -1000)
                        {
                            speedY *= -1;
                        }
                        else if (collisionResult != 0)
                        {
                            speedY += collisionResult;
                            speedX *= -1;
                            if (speedX > 0) {
                                speedX += Math.Abs(collisionResult) - 1;
                            }
                            if (speedX < 0)
                            {
                                speedX -= Math.Abs(collisionResult) - 1;
                            }
                            SoundEffect _contactSound = _contactSounds[new Random().Next(0, _contactSounds.Count)];
                            _contactSound.CreateInstance().Play();
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
            sourceRectangle = new Rectangle((texture.Width / framesPerRow) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height / frameRows);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            if (animationFrame < explodeFrames || isExploding)
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

                //Texture2D _texture;

                //_texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
                //_texture.SetData(new Color[] { Color.Red });

                //spriteBatch.Draw(_texture, GetCollisionBoundaries(), Color.White);
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

        public override int IsCollisionPoint(Rectangle other)
        {
            if (GetCollisionBoundaries().Intersects(other))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public abstract int DamageValue();

        public int GetPlayerDamaged()
        {
            if (!isExploding)
            {
                return 0;
            }
            else if (position.X > _preferredBackBufferWidth / 2)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }
}
