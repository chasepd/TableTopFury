using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TableTopFury.Objects
{
    internal abstract class Paddle : TTFObject
    {
        protected int playerNumber;
        protected int paddleHeightModifier;
        protected int paddleWidthModifier;
        protected double _animateTimeTracker;
        protected double _speedChangeTimeTracker;
        protected int absoluteSpeed;
        protected int speedStep;
        protected int boostStep;
        protected const double _speedChangeDelay = 0.02;
        protected Texture2D boosterTexture;
        protected Texture2D normalBoosterTexture;
        protected Texture2D ultraBoosterTexture;
        protected int boosterFramesPerRow;
        protected int boosterFrameRows;
        protected Rectangle boosterSourceRectangle;
        protected int boosterAnimationFrame;
        protected double boosterTimeTracker;

        public Paddle(int playerNumber)
        {
            animationFrame = 1;
            frameRows = 1;
            framesPerRow = 7;
            boosterFramesPerRow = 5;
            boosterFrameRows = 1;
            boosterAnimationFrame = 1;
            _animateTimeTracker = 0.0;
            speedX = 0;
            speedY = 0;
            if (playerNumber > 2 || playerNumber < 1)
            {
                throw new ArgumentOutOfRangeException("playerNumber value of " + playerNumber + " was outside of expected range. Valid values are 1 or 2.");
            }
            this.playerNumber = playerNumber;
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            scaleModifier = (float)(0.25 * graphics.PreferredBackBufferHeight / 480);
            paddleWidthModifier = (int)(scaleModifier * 64);
            paddleHeightModifier = (int)(scaleModifier * 192);
            if (graphics.PreferredBackBufferWidth * 9 > graphics.PreferredBackBufferHeight * 16)
            {
                if (playerNumber == 1)
                {
                    position = new Vector2(paddleWidthModifier + 10 + (graphics.PreferredBackBufferWidth - (16 * graphics.PreferredBackBufferHeight) / 9) / 2, graphics.PreferredBackBufferHeight / 2);
                }
                else
                {
                    position = new Vector2(graphics.PreferredBackBufferWidth - (paddleWidthModifier + 10) - (graphics.PreferredBackBufferWidth - (16 * graphics.PreferredBackBufferHeight) / 9) / 2, graphics.PreferredBackBufferHeight / 2);
                }
            }
            else
            {
                if (playerNumber == 1)
                {
                    position = new Vector2(paddleWidthModifier + 10, graphics.PreferredBackBufferHeight / 2);
                }
                else
                {
                    position = new Vector2(graphics.PreferredBackBufferWidth - (paddleWidthModifier + 10), graphics.PreferredBackBufferHeight / 2);
                }
            }
            absoluteSpeed = (int)(40 * scaleModifier) * 4;
            speedStep = (int)(1 * scaleModifier) * 4;
            boostStep = (int)(5 * scaleModifier) * 4;

            if (absoluteSpeed < 1)
            {
                absoluteSpeed = 1;
            }
            if (speedStep < 1)
            {
                speedStep = 1;
            }
            if (boostStep < 1)
            {
                boostStep = 1;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            normalBoosterTexture = content.Load<Texture2D>("BoosterFlames");
            ultraBoosterTexture = content.Load<Texture2D>("UltraBoosterFlames");
            boosterTexture = normalBoosterTexture;
        }

        protected void UpwardMovement(bool boost)
        {
            if (_speedChangeTimeTracker > _speedChangeDelay)
            {
                if (!boost)
                {
                    boosterTexture = normalBoosterTexture;
                    speedY -= speedStep;
                }
                else
                {
                    boosterTexture = ultraBoosterTexture;
                    speedY -= boostStep;
                }
                if (speedY < -1 * absoluteSpeed)
                {
                    speedY = -1 * absoluteSpeed;
                }
                _speedChangeTimeTracker = 0;
            }
        }

        protected void DownwardMovement(bool boost)
        {
            if (_speedChangeTimeTracker > _speedChangeDelay)
            {
                if (!boost)
                {
                    speedY += speedStep;
                    boosterTexture = normalBoosterTexture;
                }
                else
                {
                    boosterTexture = ultraBoosterTexture;
                    speedY += boostStep;
                }
                if (speedY > absoluteSpeed)
                {
                    speedY = absoluteSpeed;
                }
                _speedChangeTimeTracker = 0;
            }
        }

        protected void SlowToStop()
        {
            if (_speedChangeTimeTracker > _speedChangeDelay)
            {
                if (speedY > 0)
                {
                    speedY -= speedStep;
                    if (speedY < 0)
                    {
                        speedY = 0;
                    }
                }
                if (speedY < 0)
                {
                    speedY += speedStep;
                    if (speedY > 0)
                    {
                        speedY = 0;
                    }
                }
                _speedChangeTimeTracker = 0;
            }
        }


        public override void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<TTFObject> objects)
        {
            _animateTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
            _speedChangeTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;
            boosterTimeTracker += gameTime.ElapsedGameTime.TotalSeconds;

            position.Y += speedY;
            position.X += speedX;

            if (position.Y < 0 + paddleHeightModifier)
            {
                position.Y = 0 + paddleHeightModifier;
                speedY = 0;
            }

            if (position.Y > graphics.PreferredBackBufferHeight - paddleHeightModifier)
            {
                position.Y = graphics.PreferredBackBufferHeight - paddleHeightModifier;
                speedY = 0;
            }

            if (_animateTimeTracker > .4)
            {
                animationFrame++;
                _animateTimeTracker = 0.0;
            }

            if (animationFrame >= framesPerRow)
            {
                animationFrame = 1;
            }

            if (boosterTimeTracker > 0.4)
            {
                boosterAnimationFrame++;
                if (boosterAnimationFrame >= boosterFramesPerRow) 
                { 
                    boosterAnimationFrame = 1;
                }
                boosterTimeTracker = 0.0;
            }

            sourceRectangle = new Rectangle((texture.Width / framesPerRow) * (animationFrame - 1), 0, texture.Width / framesPerRow, texture.Height);
            boosterSourceRectangle = new Rectangle(boosterTexture.Width / boosterFramesPerRow * (boosterAnimationFrame - 1), 0, boosterTexture.Width/ boosterFramesPerRow, boosterTexture.Height);
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
                 0.2f
                );

            SpriteEffects boosterRotation = SpriteEffects.None;
            Vector2 boosterPosition = new Vector2(position.X, position.Y + (texture.Height / 2 * scaleModifier) + (boosterTexture.Height / 2 * scaleModifier));

            if (speedY > 0)
            {
                boosterRotation = SpriteEffects.FlipVertically;
                boosterPosition = new Vector2(position.X, position.Y - (texture.Height / 2 * scaleModifier) - (boosterTexture.Height / 2 * scaleModifier));
            }

            if(speedY < 0 || speedY > 0)
            {
                spriteBatch.Draw(
                 boosterTexture,
                 boosterPosition,
                 boosterSourceRectangle,
                 Color.White,
                 0,
                 new Vector2(boosterTexture.Width / (boosterFramesPerRow * 2), boosterTexture.Height / (boosterFrameRows * 2)),
                 scaleModifier,
                 boosterRotation,
                 0.1f
                );
            }

            //Texture2D _texture;

            //_texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            //_texture.SetData(new Color[] { Color.Red });

            //spriteBatch.Draw(_texture, new Rectangle(
            //    (int)(position.X),
            //    (int)(position.Y),
            //    5,
            //    5), Color.White);

        }

        public override int GetCollisionIntensity(Rectangle other)
        {
            Rectangle thisSprite = new Rectangle((int)position.X - paddleWidthModifier, (int)position.Y - paddleHeightModifier, GetWidth(), GetHeight());
            Rectangle topEdge = new Rectangle(
                (int)(position.X - paddleWidthModifier + (paddleWidthModifier * 0.2f)),
                (int)position.Y - paddleHeightModifier,
                (int)(GetWidth() - (paddleWidthModifier * 0.4f)),
                (int)(GetHeight() * 0.1));
            Rectangle bottomEdge = new Rectangle(
                (int)(position.X - paddleWidthModifier + (paddleWidthModifier * 0.2f)),
                (int)(position.Y + paddleHeightModifier - (paddleHeightModifier * 0.1f)),
                (int)(GetWidth() - (paddleWidthModifier * 0.4f)),
                (int)(GetHeight() * 0.1));
            
            if (topEdge.Intersects(other) || bottomEdge.Intersects(other))
            {
                return -1000;
            }
            else if (thisSprite.Intersects(other))
            {
                if (Math.Abs(other.Center.X - thisSprite.Center.X) > (0.5 * other.Width) && other.Center.Y - thisSprite.Center.Y < 0 &&
                    Math.Abs(other.Center.Y - thisSprite.Center.Y) < thisSprite.Height / 4)
                {
                    return -1;
                }
                else if ((Math.Abs(other.Center.X - thisSprite.Center.X) > (0.5 * other.Width) && other.Center.Y - thisSprite.Center.Y < 0 &&
                    Math.Abs(other.Center.Y - thisSprite.Center.Y) > thisSprite.Height / 4))
                {
                    return -2;
                }
                else if (Math.Abs(other.Center.X - thisSprite.Center.X) > (0.5 * other.Width) && other.Center.Y - thisSprite.Center.Y > 0 &&
                    Math.Abs(other.Center.Y - thisSprite.Center.Y) < thisSprite.Height / 4)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                //if(other.X > thisSprite.X && other.X < thisSprite.X + thisSprite.Width &&
                //    other.Y > thisSprite.Y && other.Y < thisSprite.Y + thisSprite.Height)
                //{
                //    Debug.WriteLine("Something funky is going on here!");
                //}
                return 0;
            }
        }
    }
}
