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
        private bool _recentlyCollided;
        private int _preferredBackBufferWidth;
        private int _preferredBackBufferHeight;
        public bool isExploding;
        const int explodeFrames = 14;
        const int ballFrames = 9;
        protected ContentManager _contentManager;
        public Texture2D explosionTexture;
        public Texture2D ballTexture;
        protected SoundEffect _explosionSound;
        protected List<SoundEffect> _contactSounds;
        public Ball() 
        {
            _explosionTimeTracker = 0.0;
            _rotationTimeTracker = 0.0;
            frameRows = 1;
            framesPerRow = ballFrames;
            isExploding = false;
            _contactSounds = new List<SoundEffect>();
            _recentlyCollided = false;
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
            _recentlyCollided = false;
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            scaleModifier = (float)(graphics.PreferredBackBufferHeight / 480.0 / 3.0);
            position = new Vector2(graphics.PreferredBackBufferWidth / 2 - ((texture.Width * scaleModifier / framesPerRow) / 2),
               graphics.PreferredBackBufferHeight / 2 - ((texture.Height * scaleModifier / frameRows) / 2));

            _preferredBackBufferHeight = graphics.PreferredBackBufferHeight;
            _preferredBackBufferWidth = graphics.PreferredBackBufferWidth;
            ResetBall();
        }        

        public void Explode()
        {
            texture = explosionTexture;
            isExploding = true;
            rotation = 0;
            framesPerRow = explodeFrames;
            _explosionSound.CreateInstance().Play();
            scaleModifier *= 2;
        }

        private int GetLeftEdge(GraphicsDeviceManager graphics)
        {
            if (graphics.PreferredBackBufferWidth * 9 > graphics.PreferredBackBufferHeight * 16)
            {
                return (graphics.PreferredBackBufferWidth - (16 * graphics.PreferredBackBufferHeight) / 9) / 2;
            }
            else
            {
                return 0;
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
                return graphics.PreferredBackBufferWidth;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            _contentManager = content;
        }

        public Rectangle GetCollisionBoundaries()
        {
            //Position appears to be returning center. Remove half the ball size to get the correct coordinates for the top left of the collision rectangle.
            return new Rectangle((int)position.X - (int)(texture.Width / framesPerRow / 2 * scaleModifier), 
                (int)position.Y - (int)(texture.Height / frameRows / 2 * scaleModifier), 
                (int)(texture.Width / framesPerRow * scaleModifier), 
                (int)(texture.Height / frameRows * scaleModifier));            
            
        }

        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            if (speedX > GameState.Paddles[0].GetWidth())
            {
                speedX = (int)(0.75 * GameState.Paddles[0].GetWidth());
            }
            if (isExploding)
            {
                _recentlyCollided = false;
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
                if (_ballAnimateTimeTracker >= 0.6)
                {
                    animationFrame += 1;
                    if (animationFrame > ballFrames)
                    {
                        animationFrame = 1;
                    }
                    _ballAnimateTimeTracker = 0.0;
                } 

                if (position.X < GameState.Paddles[0].position.X)
                {
                    Explode();
                }
                if (position.X > GameState.Paddles[1].position.X)
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
                bool collision = false;
                foreach (TTFObject obj in objects)
                {
                    if (obj is Paddle || obj is Board)
                    {
                        int collisionResult = obj.GetCollisionIntensity(GetCollisionBoundaries());
                        int collisionResultAfterMove = obj.GetCollisionIntensity(new Rectangle(
                            (int)(position.X + (speedY * scaleModifier) - (this.GetWidth() / 2.0)),
                            (int)(position.Y + (speedY * scaleModifier) - (this.GetHeight() / 2.0)),
                            (int)this.GetWidth(), (int)this.GetHeight()));

                        if (collisionResult == 0 && collisionResultAfterMove != 0 && obj is Paddle)
                        {
                            collisionResult = collisionResultAfterMove;
                        }

                        if (collisionResult == -1000)
                        {
                            if (!_recentlyCollided)
                            {
                                speedY *= -1;
                                speedX *= -1;
                                //speedX /= Math.Abs(speedX);
                                //speedX *= (int)(scaleModifier * 5);
                                collision = true;
                            }
                            _recentlyCollided = true;
                        }
                        else if (collisionResult == -50)
                        {
                            if (!_recentlyCollided)
                            {
                                speedY *= -1;
                                collision = true;
                            }
                            _recentlyCollided = true;
                        }
                        else if (collisionResult != 0)
                        {
                            if (!_recentlyCollided)
                            {
                                speedY += collisionResult;
                                speedX *= -1;
                                if (speedX > 0)
                                {
                                    speedX += Math.Abs(collisionResult);
                                }
                                if (speedX < 0)
                                {
                                    speedX -= Math.Abs(collisionResult);
                                }
                                collision = true;
                            }
                            _recentlyCollided = true;
                        }
                        else
                        {
                            _recentlyCollided = false;
                        }
                    }
                }
                
                Board board = GameState.Board;
                if (position.Y + (speedY * scaleModifier) > board.position.Y + (board.GetHeight() / 2) - (this.GetHeight() / 2f))
                {
                    position.Y = board.position.Y + (board.GetHeight() / 2f) - (this.GetHeight() / 2f);
                    speedY *= -1;
                    collision = true;
                }
                else if (position.Y + (speedY * scaleModifier) < board.position.Y - (board.GetHeight() / 2) + (this.GetHeight() / 2f))
                {
                    position.Y = board.position.Y - (board.GetHeight() / 2) + (this.GetHeight() / 2f);
                    speedY *= -1;
                    collision = true;
                }
                else
                {
                    position.Y += (int)(speedY * scaleModifier);
                }

                //List<Paddle> paddles = GameState.Paddles;
                //foreach (Paddle paddle in paddles)
                //{
                //    if (paddle.GetCollisionIntensity(new Rectangle(
                //        (int)(position.X + (speedY * scaleModifier) - (this.GetWidth() / 2.0)),
                //        (int)(position.Y + (speedY * scaleModifier) - (this.GetHeight() / 2.0)),
                //        this.GetWidth(), this.GetHeight())) != 0)
                //    {
                //        collision = true;
                //        speedX = -1;
                //    }
                //}
                position.X += (int)(speedX * scaleModifier);

                _rotationTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
                if (_rotationTimeTracker >= 0.05 && !isExploding)
                {
                    rotation += speedX / 2 + speedY / 2;
                    _rotationTimeTracker = 0;
                }

                if (collision)
                {
                    SoundEffect _contactSound = _contactSounds[new Random().Next(0, _contactSounds.Count)];
                    _contactSound.CreateInstance().Play();
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
                //Texture2D _texture;

                //_texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
                //_texture.SetData(new Color[] { Color.Red });

                //spriteBatch.Draw(_texture, GetCollisionBoundaries(), Color.White);
                spriteBatch.Draw(
                 texture,
                 position,
                 sourceRectangle,
                 Color.White,
                 MathHelper.ToRadians(rotation),
                 new Vector2(texture.Width / (framesPerRow * 2), texture.Height / (frameRows * 2)),
                 scaleModifier,
                 SpriteEffects.None,
                 0.2f
                );


            }
            else
            {
                animationFrame = 1;
                scaleModifier /= 2;
                ResetBall();                
            }
        }

        public override int GetCollisionIntensity(Rectangle other)
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

        private void ResetBall()
        {
            animationFrame = 1;
            rotation = 0;
            position = new Vector2(_preferredBackBufferWidth / 2,
             _preferredBackBufferHeight / 2);
            speedX = 5;
            speedX += new Random().Next(0, 20);
            if (new Random().Next(1, 3) == 2)
            {
                speedX *= -1;
            }
            speedY = new Random().Next(-1, 2);
            speedX = (int)(speedX * scaleModifier);
            speedY = (int)(speedY * scaleModifier);
        }

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
