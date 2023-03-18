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
        const float scaleModifier = .25f;
        private double _animateTimeTracker;
        private double _speedChangeTimeTracker;
        private int absoluteSpeed;
        private int speedStep;
        private int boostStep;
        private int playerNumber;
        const int paddleHeightModifier = (int)(scaleModifier * 192);
        const int paddleWidthModifier = (int)(scaleModifier * 64);
        private const double _speedChangeDelay = 0.02;
        public PlayerPaddle(int playerNumber) : base() 
        {
            animationFrame = 1;
            frameRows = 1;
            framesPerRow = 7;
            _animateTimeTracker = 0.0;
            speedX = 0;
            speedY = 0;
            absoluteSpeed = 40;
            speedStep = 1;
            boostStep = 5;
            if (playerNumber > 2 || playerNumber < 1)
            {
                throw new ArgumentOutOfRangeException("playerNumber value of " + playerNumber + " was outside of expected range. Valid values are 1 or 2.");
            }
            this.playerNumber = playerNumber;
        }

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            if (playerNumber == 1)
            {
                position = new Vector2(0 + paddleWidthModifier + 10, graphics.PreferredBackBufferHeight / 2);
            }
            else
            {
                position = new Vector2(graphics.PreferredBackBufferWidth - (paddleWidthModifier + 10), graphics.PreferredBackBufferHeight / 2);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("BasicPaddle");
        }

        private void UpwardMovement(bool boost)
        {
            if (_speedChangeTimeTracker > _speedChangeDelay)
            {
                if (!boost)
                {
                    speedY -= speedStep;
                }
                else
                {
                    speedY -= boostStep;
                }
                if (speedY < -1 * absoluteSpeed)
                {
                    speedY = -1 * absoluteSpeed;
                }
                _speedChangeTimeTracker = 0;
            }
        }

        private void DownwardMovement(bool boost)
        {
            if (_speedChangeTimeTracker > _speedChangeDelay)
            {
                if (!boost)
                {
                    speedY += speedStep;
                }
                else
                {
                    speedY += boostStep;
                }
                if (speedY > absoluteSpeed)
                {
                    speedY = absoluteSpeed;
                }
                _speedChangeTimeTracker = 0;
            }
        }

        private void SlowToStop()
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

            var kstate = Keyboard.GetState();
            if (playerNumber == 1)
            {
                if (kstate.IsKeyDown(Keys.W))
                {
                    UpwardMovement(kstate.IsKeyDown(Keys.LeftShift));                      
                }
                else if (kstate.IsKeyDown(Keys.S))
                {
                    DownwardMovement(kstate.IsKeyDown(Keys.LeftShift));
                }
                else
                {
                    SlowToStop();
                }
            }
            else
            {
                if (kstate.IsKeyDown(Keys.Up))
                {
                    UpwardMovement(kstate.IsKeyDown(Keys.RightShift));
                }
                else if (kstate.IsKeyDown(Keys.Down))
                {
                    DownwardMovement(kstate.IsKeyDown(Keys.RightShift));
                }
                else
                {
                    SlowToStop();
                }
            }

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

            //Texture2D _texture;

            //_texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            //_texture.SetData(new Color[] { Color.Red });

            //spriteBatch.Draw(_texture, new Rectangle((int)position.X - paddleWidthModifier, (int)position.Y - paddleHeightModifier, paddleWidthModifier * 2, paddleHeightModifier * 2), Color.White);
        }

        public override int IsCollisionPoint(Rectangle other)
        {
            Rectangle thisSprite = new Rectangle((int)position.X - paddleWidthModifier, (int)position.Y - paddleHeightModifier, paddleWidthModifier * 2, paddleHeightModifier * 2);

            if(thisSprite.Intersects(other))
            {

                if (Math.Abs(other.Top - thisSprite.Bottom) < 10 && Math.Abs(other.Center.X - thisSprite.Center.X) < other.Width)
                {
                    //Signal a speed direction reversal
                    return -1000;
                }
                else if (Math.Abs(other.Center.X - thisSprite.Center.X) > (0.5 * other.Width) && other.Center.Y - thisSprite.Center.Y < 0 &&
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
                return 0;
            }
        }
    }
}
